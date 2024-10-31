using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using XFEExtension.NetCore.FileExtension;

namespace SpaceEngineersBlueprintEditor.Utilities;

public static class BlueprintsManager
{
    public static List<BlueprintInfo> LocalBlueprints { get; set; } = [];
    public static List<BlueprintInfo> CloudBlueprints { get; set; } = [];
    public static List<BlueprintInfo> WorkshopBlueprints { get; set; } = [];
    public static List<BlueprintInfo> LoadLocalBlueprints() => LoadBlueprintsFrom(SpaceEngineersPath.LocalBlueprints);
    public static List<BlueprintInfo> LoadCloudBlueprints() => LoadBlueprintsFrom(SpaceEngineersPath.CloudBlueprints);
    public static List<BlueprintInfo> LoadWorkshopBlueprints() => LoadBlueprintsFrom(SpaceEngineersPath.WorkshopBlueprints);
    public static List<BlueprintInfo> LoadBlueprintsFrom(string path)
    {
        var list = new List<BlueprintInfo>();
        foreach (var directory in Directory.GetDirectories(path))
        {
            var blueprintPath = $@"{directory}\bp.sbc";
            var imagePath = $@"{directory}\thumb.png";
            var hasBlueprint = File.Exists(blueprintPath);
            list.Add(new(imagePath, hasBlueprint, File.Exists(imagePath), Path.GetFileName(directory)!, hasBlueprint ? $"{new FileInfo(blueprintPath).Length.FileSize()}" : string.Empty, blueprintPath));
        }
        return list;
    }
    public static void LoadBlueprints()
    {
        LocalBlueprints = LoadLocalBlueprints();
        CloudBlueprints = LoadCloudBlueprints();
        WorkshopBlueprints = LoadWorkshopBlueprints();
    }
    public static async Task LoadBlueprintsAsync() => await Task.Run(LoadBlueprints);
}
