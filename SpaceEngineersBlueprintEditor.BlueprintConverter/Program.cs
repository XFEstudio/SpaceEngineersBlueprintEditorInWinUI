using Newtonsoft.Json;
using System.Xml.Serialization;
using VRage.Game;

public class Program
{
    private static void Main(string[] args)
    {
        foreach (var arg in args)
        {
            try
            {
                if (File.Exists(arg))
                {
                    if (JsonConvert.DeserializeObject<MyObjectBuilder_Definitions>(File.ReadAllText(arg), new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    }) is MyObjectBuilder_Definitions myObjectBuilder_Definitions)
                    {
                        using var stream = new MemoryStream();
                        var serializer = new XmlSerializer(typeof(MyObjectBuilder_Definitions));
                        serializer.Serialize(stream, myObjectBuilder_Definitions);
                        stream.Position = 0;
                        File.WriteAllText($"{arg}.sbc", new StreamReader(stream).ReadToEnd());
                        Console.Write("Successful");
                    }
                    else
                    {
                        Console.Write($"Error:File: {arg} serialize failed");
                    }
                }
                else
                {
                    Console.Write($"Error:File: {arg} not exist!");
                }
            }
            catch (Exception ex)
            {
                Console.Write($"Error:{ex.Message}");
            }
        }
    }
}