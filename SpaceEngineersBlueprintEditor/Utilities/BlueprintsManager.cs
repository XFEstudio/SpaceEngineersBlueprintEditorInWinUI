using SpaceEngineersBlueprintEditor.Model;
using XFEExtension.NetCore.FileExtension;

namespace SpaceEngineersBlueprintEditor.Utilities;

/// <summary>
/// 蓝图管理器
/// </summary>
public static class BlueprintsManager
{
    /// <summary>
    /// 本地蓝图
    /// </summary>
    public static List<BlueprintInfo> LocalBlueprints { get; set; } = [];
    /// <summary>
    /// 云端蓝图
    /// </summary>
    public static List<BlueprintInfo> CloudBlueprints { get; set; } = [];
    /// <summary>
    /// 工坊蓝图
    /// </summary>
    public static List<BlueprintInfo> WorkshopBlueprints { get; set; } = [];
    /// <summary>
    /// 加载本地蓝图
    /// </summary>
    /// <returns></returns>
    public static List<BlueprintInfo> LoadLocalBlueprints() => LoadBlueprintsFrom(SpaceEngineersPath.LocalBlueprints);
    /// <summary>
    /// 加载云端蓝图
    /// </summary>
    /// <returns></returns>
    public static List<BlueprintInfo> LoadCloudBlueprints() => LoadBlueprintsFrom(SpaceEngineersPath.CloudBlueprints);
    /// <summary>
    /// 加载工坊蓝图
    /// </summary>
    /// <returns></returns>
    public static List<BlueprintInfo> LoadWorkshopBlueprints() => LoadBlueprintsFrom(SpaceEngineersPath.WorkshopBlueprints);
    /// <summary>
    /// 从指定位置加载蓝图
    /// </summary>
    /// <param name="path">指定目录</param>
    /// <returns></returns>
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
    /// <summary>
    /// 加载蓝图
    /// </summary>
    public static void LoadBlueprints()
    {
        LocalBlueprints = LoadLocalBlueprints();
        CloudBlueprints = LoadCloudBlueprints();
        WorkshopBlueprints = LoadWorkshopBlueprints();
    }
    /// <summary>
    /// 异步加载蓝图
    /// </summary>
    /// <returns>异步任务</returns>
    public static async Task LoadBlueprintsAsync() => await Task.Run(LoadBlueprints);
}
