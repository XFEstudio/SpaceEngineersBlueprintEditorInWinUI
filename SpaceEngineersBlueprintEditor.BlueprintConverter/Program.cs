using Newtonsoft.Json;
using System.Xml.Serialization;
using VRage.Game;

public class Program
{
    private static void Main(string[] args)
    {
        foreach (var arg in args)
        {
            Console.Write($"Info:开始转换文件：{arg}...");
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
                        Console.Write($"Success:转换完成：{Path.GetFileName(arg)}");
                    }
                    else
                    {
                        Console.Write($"Error:文件：{arg} 序列化失败！");
                    }
                }
                else
                {
                    Console.Write($"Error:文件：{arg} 不存在！");
                }
            }
            catch (Exception ex)
            {
                Console.Write($"Error:{ex.Message}");
            }
        }
    }
}