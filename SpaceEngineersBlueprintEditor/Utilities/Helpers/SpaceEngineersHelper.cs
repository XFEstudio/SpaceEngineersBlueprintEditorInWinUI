using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.Win32;
using Sandbox.Definitions;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.SpaceEngineersCore;
using System.Collections;
using System.Reflection;
using VRage.Collections;
using VRage.Game;
using XFEExtension.NetCore.FileExtension;

namespace SpaceEngineersBlueprintEditor.Utilities.Helpers;

/// <summary>
/// 太空工程师帮助类
/// </summary>
public static class SpaceEngineersHelper
{
    /// <summary>
    /// 加载完成事件
    /// </summary>
    public static event EventHandler? LoadComplete;
    /// <summary>
    /// 是否已经加载完成
    /// </summary>
    public static bool IsLoadComplete { get; private set; } = false;
    /// <summary>
    /// 所有类型定义集
    /// </summary>
    public static DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> AllDefinitions => MyDefinitionManager.Static.GetAllDefinitions();
    /// <summary>
    /// 方块定义集
    /// </summary>
    public static IEnumerable<MyCubeBlockDefinition> CubeBlockDefinitions => AllDefinitions.Where(definition => definition is MyCubeBlockDefinition).Cast<MyCubeBlockDefinition>();
    /// <summary>
    /// 组件定义集
    /// </summary>
    public static IEnumerable<MyComponentDefinition> ComponentDefinitions => AllDefinitions.Where(definition => definition is MyComponentDefinition).Cast<MyComponentDefinition>();
    /// <summary>
    /// 物品定义集
    /// </summary>
    public static IEnumerable<MyPhysicalItemDefinition> ItemDefinitions => AllDefinitions.Where(definition => definition is MyPhysicalItemDefinition).Cast<MyPhysicalItemDefinition>();
    /// <summary>
    /// 场景定义集
    /// </summary>
    public static IEnumerable<MyScenarioDefinition> ScenarioDefinition => AllDefinitions.Where(definition => definition is MyScenarioDefinition).Cast<MyScenarioDefinition>();

    /// <summary>
    /// 加载蓝图模型
    /// </summary>
    /// <param name="blueprintFile">蓝图文件路径</param>
    /// <returns>蓝图模型（异步）</returns>
    public static async Task<BlueprintModel?> LoadBlueprintModel(string blueprintFile) => new()
    {
        ViewData = LoadBlueprintInfo(blueprintFile)?.ToBlueprintInfoViewData(),
        BlueprintDefinitions = await LoadBlueprintAsync(blueprintFile)
    };

    /// <summary>
    /// 异步加载蓝图定义
    /// </summary>
    /// <param name="blueprintFile">蓝图文件路径</param>
    /// <returns>蓝图定义（异步）</returns>
    public static async Task<MyObjectBuilder_Definitions> LoadBlueprintAsync(string blueprintFile) => await Task.Run(() => SpaceEngineerDefinitions.Load<MyObjectBuilder_Definitions>(blueprintFile));

    /// <summary>
    /// 加载蓝图信息
    /// </summary>
    /// <param name="blueprintFile">蓝图文件路径</param>
    /// <returns>蓝图信息</returns>
    public static BlueprintInfo? LoadBlueprintInfo(string blueprintFile)
    {
        var rootPath = Path.GetDirectoryName(blueprintFile);
        var imagePath = $@"{rootPath}\thumb.png";
        return new(imagePath, File.Exists(blueprintFile), File.Exists(imagePath), Path.GetFileName(rootPath)!, new FileInfo(blueprintFile).Length.FileSize(), blueprintFile);
    }

    /// <summary>
    /// 分析蓝图定义
    /// </summary>
    /// <param name="blueprintPropertyViewData">蓝图属性视图数据</param>
    /// <param name="exceptedName">不希望分析的属性名称列表</param>
    public static void AnalyzeBlueprint(BlueprintPropertyViewData blueprintPropertyViewData, params string[] exceptedName)
    {
        if (CheckIfNull(blueprintPropertyViewData))
            return;
        if (IsSpecialObject(blueprintPropertyViewData))
            return;
        foreach (var member in blueprintPropertyViewData.Type!.GetMembers())
        {
            Type? type = null;
            try
            {
                if (member is FieldInfo fieldInfo && !exceptedName.Contains(fieldInfo.Name) && fieldInfo.IsPublic)
                {
                    type = fieldInfo.FieldType;
                    var value = fieldInfo.GetValue(blueprintPropertyViewData.Value);
                    var child = new BlueprintPropertyViewData()
                    {
                        Type = type,
                        Name = fieldInfo.Name,
                        Value = value,
                        Parent = blueprintPropertyViewData
                    };
                    SetDetail(child, value);
                    blueprintPropertyViewData.Children.Add(child);
                }
                else if (member is PropertyInfo propertyInfo && !exceptedName.Contains(propertyInfo.Name) && propertyInfo.CanWrite && propertyInfo.IsMemberPublic())
                {
                    type = propertyInfo.PropertyType;
                    var value = propertyInfo.GetValue(blueprintPropertyViewData.Value);
                    var child = new BlueprintPropertyViewData()
                    {
                        Type = type,
                        Name = propertyInfo.Name,
                        Value = value,
                        Parent = blueprintPropertyViewData
                    };
                    SetDetail(child, value);
                    blueprintPropertyViewData.Children.Add(child);
                }
            }
            catch (Exception ex)
            {
                blueprintPropertyViewData.Children.Add(new()
                {
                    Type = type,
                    Name = $"{"Error_Exception".GetLocalized()}: {ex.Message}",
                    Parent = blueprintPropertyViewData
                });
            }
        }
    }

    /// <summary>
    /// 检测是否为Null
    /// </summary>
    /// <param name="blueprintPropertyViewData">蓝图属性视图数据</param>
    /// <returns>是否为Null</returns>
    private static bool CheckIfNull(BlueprintPropertyViewData blueprintPropertyViewData)
    {
        if (blueprintPropertyViewData.Value is null || blueprintPropertyViewData.Type is null)
        {
            blueprintPropertyViewData.Children.Add(new()
            {
                Name = "NullObject".GetLocalized(),
                Parent = blueprintPropertyViewData
            });
            return true;
        }
        return false;
    }

    /// <summary>
    /// 是否是特殊对象
    /// </summary>
    /// <param name="blueprintPropertyViewData">蓝图属性视图数据</param>
    /// <returns>是否是特殊对象</returns>
    private static bool IsSpecialObject(BlueprintPropertyViewData blueprintPropertyViewData)
    {
        if (blueprintPropertyViewData.Type!.IsAssignableTo(typeof(IEnumerable)))
        {
            AddArrayOrEnumerableChildren<IEnumerable>(blueprintPropertyViewData);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 添加数组或者枚举类型子数据
    /// </summary>
    /// <typeparam name="T">目标枚举类型</typeparam>
    /// <param name="blueprintPropertyViewData">蓝图属性视图数据</param>
    private static void AddArrayOrEnumerableChildren<T>(BlueprintPropertyViewData blueprintPropertyViewData) where T : IEnumerable
    {
        foreach (var item in (T)blueprintPropertyViewData.Value!)
        {
            var type = item.GetType();
            var child = new BlueprintPropertyViewData()
            {
                Type = type,
                Name = type.Name,
                Value = item,
                Parent = blueprintPropertyViewData
            };
            SetDetail(child, item);
            blueprintPropertyViewData.Children.Add(child);
        }
    }

    /// <summary>
    /// 设置定义细节
    /// </summary>
    /// <param name="blueprintPropertyViewData">蓝图属性视图数据</param>
    /// <param name="value">目标定义值</param>
    public static void SetDetail(BlueprintPropertyViewData blueprintPropertyViewData, object? value)
    {
        if (value is MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock)
        {
            var cubeBlock = MyDefinitionManager.Static.GetCubeBlockDefinition(myObjectBuilder_CubeBlock);
            blueprintPropertyViewData.Name = cubeBlock.DisplayNameText;
            blueprintPropertyViewData.CubeImage = new BitmapImage(new(@$"{AppPath.DefinitionImages}\{FileHelper.ChangeExtension(cubeBlock.Icons[0], "png")}"));
            blueprintPropertyViewData.CustomData = value is MyObjectBuilder_TerminalBlock myObjectBuilder_TerminalBlock ? myObjectBuilder_TerminalBlock.CustomName : string.Empty;
        }
        else if (value is MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid)
        {
            blueprintPropertyViewData.CustomData = myObjectBuilder_CubeGrid.DisplayName;
        }
        else if (blueprintPropertyViewData.IsMultiEnum)
        {
            var stackPanel = new StackPanel
            {
                DataContext = blueprintPropertyViewData
            };
            foreach (var enumItem in blueprintPropertyViewData.EnumValues)
            {
                stackPanel.Children.Add(new CheckBox
                {
                    Content = enumItem,
                    IsChecked = blueprintPropertyViewData.Value?.ToString()?.Contains(enumItem)
                });
            }
            blueprintPropertyViewData.CustomData = stackPanel;
        }
        else if (blueprintPropertyViewData.Type is not null && blueprintPropertyViewData.Type.IsAssignableTo(typeof(IEnumerable)))
        {
            if (value is null)
            {
                blueprintPropertyViewData.CustomData = "EmptyList".GetLocalized();
            }
            else
            {
                var count = 0;
                foreach (var item in (IEnumerable)value)
                    count++;
                blueprintPropertyViewData.CustomData = $"{"Amount".GetLocalized()}: {count}";
            }
        }
        else
        {
            blueprintPropertyViewData.CustomData = blueprintPropertyViewData.Name;
        }
    }

    /// <summary>
    /// 异步加载定义集列表
    /// </summary>
    /// <returns>异步任务</returns>
    public static async Task LoadDefinitionViewDataListAsync()
    {
        await Helper.Wait(() => Initializer.IsDefinitionsLoadComplete);
        foreach (var definition in AllDefinitions)
            if (definition is not null && definition.Icons is not null && definition.Icons.Length > 0)
            {
                var originalPath = @$"{SpaceEngineersPath.SpaceEngineerContentPath}\{definition.Icons[0]}";
                var targetPath = @$"{AppPath.DefinitionImages}\{FileHelper.ChangeExtension(definition.Icons[0], "png")}";
                FileHelper.AutoCreateDirectory(targetPath);
                if (!File.Exists(targetPath))
                    FileHelper.ToBitmap(originalPath, targetPath);
            }
        IsLoadComplete = true;
        LoadComplete?.Invoke(null, EventArgs.Empty);
    }

    /// <summary>
    /// 获取定义集
    /// </summary>
    /// <param name="m_hash">哈希值</param>
    /// <returns>定义集</returns>
    public static MyDefinitionBase? GetDefinition(int m_hash) => AllDefinitions.First(definition => definition.Id.SubtypeId.m_hash == m_hash);

    /// <summary>
    /// 获取游戏的根目录路径
    /// </summary>
    /// <returns>游戏的根目录路径</returns>
    public static string? GetGameRootPath()
    {
        var path = GetGameRootPath(@"C:\", @"D:\", @"E:\", @"F:\", @"G:\", @"H:\", @"I:\", @"J:\", @"K:\", @"L:\", @"M:\", @"N:\", @"O:\", @"P:\", @"Q:\", @"R:\", @"S:\", @"T:\", @"U:\", @"V:\", @"W:\", @"X:\", @"Y:\", @"Z:\");
        if (path is not null)
            return path;
        if ((Environment.Is64BitProcess ? Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 244850", false) : Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 244850", false)) is RegistryKey registryKey && registryKey.GetValue("InstallLocation")?.ToString() is string stringValue && !string.IsNullOrEmpty(stringValue))
        {
            return stringValue;
        }
        path = GetSteamFilePath();
        if (!string.IsNullOrEmpty(path))
            return Path.Combine(path, @"SteamApps\common\SpaceEngineers");
        return null;
    }

    /// <summary>
    /// 获取游戏的根目录路径
    /// </summary>
    /// <param name="roots">需要检测的磁盘索引</param>
    /// <returns></returns>
    public static string? GetGameRootPath(params string[] roots)
    {
        foreach (var root in roots)
        {
            var targetPath = $@"{root}SteamLibrary\steamapps\common\SpaceEngineers\Bin64\SpaceEngineers.exe";
            if (File.Exists(targetPath))
                return $@"{root}SteamLibrary\steamapps\common\SpaceEngineers";
        }
        return null;
    }

    /// <summary>
    /// 获取Steam游戏文件的根目录
    /// </summary>
    /// <returns></returns>
    public static string? GetSteamFilePath()
    {
        if ((Environment.Is64BitProcess ? Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Valve\Steam", false) : Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Valve\Steam", false)) is RegistryKey registryKey)
            return registryKey.GetValue("InstallPath")?.ToString();
        else
            return null;
    }
}
