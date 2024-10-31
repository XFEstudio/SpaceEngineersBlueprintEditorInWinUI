using Sandbox.Definitions;
using SpaceEngineersBlueprintEditor.SpaceEngineersCore;

namespace SpaceEngineersBlueprintEditor.Test;

internal class Program
{
    [SMTest]
    public static void TestMethod()
    {
        Initializer.Initialize();
        var result = MyDefinitionManager.Static.GetAllDefinitions();
    }
}