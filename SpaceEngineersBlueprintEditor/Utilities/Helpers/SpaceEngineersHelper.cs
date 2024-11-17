using Sandbox.Definitions;
using VRage.Collections;
using VRage.Game;

namespace SpaceEngineersBlueprintEditor.Utilities.Helpers;

public static class SpaceEngineersHelper
{
    public static DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> AllDefinitions => MyDefinitionManager.Static.GetAllDefinitions();
    public static IEnumerable<MyCubeBlockDefinition> CubeBlockDefinitions => AllDefinitions.Where(definition => definition is MyCubeBlockDefinition).Cast<MyCubeBlockDefinition>();
    public static IEnumerable<MyComponentDefinition> ComponentDefinitions => AllDefinitions.Where(definition => definition is MyComponentDefinition).Cast<MyComponentDefinition>();
    public static IEnumerable<MyPhysicalItemDefinition> ItemDefinitions => AllDefinitions.Where(definition => definition is MyPhysicalItemDefinition).Cast<MyPhysicalItemDefinition>();
}
