using Microsoft.Win32;
using Sandbox.Definitions;
using SpaceEngineersBlueprintEditor.SpaceEngineersCore;
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
