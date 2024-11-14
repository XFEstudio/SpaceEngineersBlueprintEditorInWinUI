using Newtonsoft.Json;
using SpaceEngineersBlueprintEditor.SpaceEngineersCore;
using System.Diagnostics;
using VRage.Game;

namespace SpaceEngineersBlueprintEditor.Test;

internal class Program
{
    [SMTest]
    public static void TestMethod()
    {
        Initializer.Initialize();
        var blueprint = SpaceEngineerDefinitions.Load<MyObjectBuilder_Definitions>(@"C:\Users\XFEstudio\AppData\Roaming\SpaceEngineers\Blueprints\local\寰宇号(大型武装殖民飞船)[Infinity Edition] V6_8_6\bp.sbc");
        var process = new Process();
        process.StartInfo.FileName = @"C:\Users\XFEstudio\Desktop\work\C#\GitHub\XFEstudio\SpaceEngineersBlueprintEditor\SpaceEngineersBlueprintEditor.BlueprintConverter\bin\Debug\net48\SpaceEngineersBlueprintEditor.BlueprintConverter.exe";
        var jsonString = JsonConvert.SerializeObject(blueprint, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
        File.WriteAllText("TestJson", jsonString);
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.ArgumentList.Add("TestJson");
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = false;
        process.Start();
        do
        {
            try
            {
                Console.Write((char)process.StandardOutput.Read());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        while (!process.HasExited);
    }
}