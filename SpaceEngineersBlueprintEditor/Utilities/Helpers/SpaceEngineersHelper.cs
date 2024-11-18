using Microsoft.UI.Xaml.Media.Imaging;
using Sandbox.Definitions;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.SpaceEngineersCore;
using VRage.Collections;
using VRage.Game;

namespace SpaceEngineersBlueprintEditor.Utilities.Helpers;

public static class SpaceEngineersHelper
{
    public static event EventHandler? LoadComplete;
    public static bool IsLoadComplete { get; private set; } = false;
    public static Dictionary<MyDefinitionBase, BitmapImage?> DefinitionImageList { get; set; } = [];
    public static DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> AllDefinitions => MyDefinitionManager.Static.GetAllDefinitions();
    public static IEnumerable<MyCubeBlockDefinition> CubeBlockDefinitions => AllDefinitions.Where(definition => definition is MyCubeBlockDefinition).Cast<MyCubeBlockDefinition>();
    public static IEnumerable<MyComponentDefinition> ComponentDefinitions => AllDefinitions.Where(definition => definition is MyComponentDefinition).Cast<MyComponentDefinition>();
    public static IEnumerable<MyPhysicalItemDefinition> ItemDefinitions => AllDefinitions.Where(definition => definition is MyPhysicalItemDefinition).Cast<MyPhysicalItemDefinition>();
    public static async Task LoadDefinitionViewDataListAsync()
    {
        while (!Initializer.IsDefinitionsLoadComplete) { await Task.Delay(100); }
        foreach (var definition in AllDefinitions)
            if (definition is not null && definition.Icons is not null && definition.Icons.Length > 0)
                DefinitionImageList.Add(definition, await FileHelper.ToBitmap($@"{SpaceEngineersPath.SpaceEngineerContentPath}\{definition.Icons[0]}"));
        IsLoadComplete = true;
        LoadComplete?.Invoke(null, EventArgs.Empty);
    }
}
