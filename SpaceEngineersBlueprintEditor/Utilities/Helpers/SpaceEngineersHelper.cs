using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media.Imaging;
using Sandbox.Definitions;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.SpaceEngineersCore;
using System.Collections.Concurrent;
using System.Diagnostics;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Definitions;
using Windows.Storage.Streams;

namespace SpaceEngineersBlueprintEditor.Utilities.Helpers;

public static class SpaceEngineersHelper
{
    public static event EventHandler? LoadComplete;
    public static bool IsLoadComplete { get; private set; } = false;
    public static DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> AllDefinitions => MyDefinitionManager.Static.GetAllDefinitions();
    public static IEnumerable<MyCubeBlockDefinition> CubeBlockDefinitions => AllDefinitions.Where(definition => definition is MyCubeBlockDefinition).Cast<MyCubeBlockDefinition>();
    public static IEnumerable<MyComponentDefinition> ComponentDefinitions => AllDefinitions.Where(definition => definition is MyComponentDefinition).Cast<MyComponentDefinition>();
    public static IEnumerable<MyPhysicalItemDefinition> ItemDefinitions => AllDefinitions.Where(definition => definition is MyPhysicalItemDefinition).Cast<MyPhysicalItemDefinition>();
    public static IEnumerable<MyDlcDefinition> DLCDefinitions => AllDefinitions.Where(definition => definition is MyDlcDefinition).Cast<MyDlcDefinition>();
    public static async Task LoadDefinitionViewDataListAsync()
    {
        while (!Initializer.IsDefinitionsLoadComplete) { await Task.Delay(100); }
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
}
