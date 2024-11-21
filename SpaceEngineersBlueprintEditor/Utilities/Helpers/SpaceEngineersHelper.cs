using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.Win32;
using Sandbox.Definitions;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.SpaceEngineersCore;
using System.Collections;
using System.Reflection;
using VRage.Collections;
using VRage.Game;

namespace SpaceEngineersBlueprintEditor.Utilities.Helpers;

public static class SpaceEngineersHelper
{
    public static event EventHandler? LoadComplete;
    public static bool IsLoadComplete { get; private set; } = false;
    public static DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> AllDefinitions => MyDefinitionManager.Static.GetAllDefinitions();
    public static IEnumerable<MyCubeBlockDefinition> CubeBlockDefinitions => AllDefinitions.Where(definition => definition is MyCubeBlockDefinition).Cast<MyCubeBlockDefinition>();
    public static IEnumerable<MyComponentDefinition> ComponentDefinitions => AllDefinitions.Where(definition => definition is MyComponentDefinition).Cast<MyComponentDefinition>();
    public static IEnumerable<MyPhysicalItemDefinition> ItemDefinitions => AllDefinitions.Where(definition => definition is MyPhysicalItemDefinition).Cast<MyPhysicalItemDefinition>();
    public static IEnumerable<MyScenarioDefinition> ScenarioDefinition => AllDefinitions.Where(definition => definition is MyScenarioDefinition).Cast<MyScenarioDefinition>();

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
                    Name = $"错误异常：{ex.Message}",
                    Parent = blueprintPropertyViewData
                });
            }
        }
    }

    private static bool CheckIfNull(BlueprintPropertyViewData blueprintPropertyViewData)
    {
        if (blueprintPropertyViewData.Value is null || blueprintPropertyViewData.Type is null)
        {
            blueprintPropertyViewData.Children.Add(new()
            {
                Name = "空对象",
                Parent = blueprintPropertyViewData
            });
            return true;
        }
        return false;
    }

    private static bool IsSpecialObject(BlueprintPropertyViewData blueprintPropertyViewData)
    {
        if (blueprintPropertyViewData.Type!.IsAssignableTo(typeof(IEnumerable)))
        {
            AddArrayOrEnumerableChildren<IEnumerable>(blueprintPropertyViewData);
            return true;
        }
        return false;
    }

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
            blueprintPropertyViewData.Name = myObjectBuilder_CubeGrid.DisplayName;
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
        else if (value is not null && value.GetType().IsAssignableTo(typeof(IEnumerable)))
        {
            var count = 0;
            foreach (var item in (IEnumerable)value)
                count++;
            blueprintPropertyViewData.CustomData = $"数量：{count}";
        }
    }

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

    public static MyDefinitionBase? GetDefinition(int m_hash) => AllDefinitions.First(definition => definition.Id.SubtypeId.m_hash == m_hash);

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

    public static string? GetSteamFilePath()
    {
        if ((Environment.Is64BitProcess ? Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Valve\Steam", false) : Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Valve\Steam", false)) is RegistryKey registryKey)
            return registryKey.GetValue("InstallPath")?.ToString();
        else
            return null;
    }
}
