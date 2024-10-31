using VRage.ObjectBuilders;
using VRage.ObjectBuilders.Private;

namespace SpaceEngineersBlueprintEditor.SpaceEngineersCore;

public class SpaceEngineerDefinitions
{
    public static T Load<T>(string path) where T : MyObjectBuilder_Base
    {
        MyObjectBuilderSerializerKeen.DeserializeXML(path, out T objectBuilder);
        return objectBuilder;
    }
}
