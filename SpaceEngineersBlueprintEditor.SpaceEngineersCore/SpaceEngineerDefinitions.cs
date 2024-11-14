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

    public static string Serialize<T>(MyObjectBuilder_Base item) where T : MyObjectBuilder_Base
    {
        using var outStream = new MemoryStream();
        if (MyObjectBuilderSerializerKeen.SerializeXML(outStream, item))
        {
            outStream.Position = 0;
            var streamReader = new StreamReader(outStream);
            return streamReader.ReadToEnd();
        }
        return string.Empty;
    }
}
