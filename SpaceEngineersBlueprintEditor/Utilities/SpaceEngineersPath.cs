using XFEExtension.NetCore.AutoPath;

namespace SpaceEngineersBlueprintEditor.Utilities;

public partial class SpaceEngineersPath
{
    [AutoPath]
    private static readonly string userGameDataRoot = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\SpaceEngineers";
    [AutoPath]
    private static readonly string blueprintsRoot = $@"{UserGameDataRoot}/Blueprints";
    [AutoPath]
    private static readonly string localBlueprints = $@"{BlueprintsRoot}/local";
    [AutoPath]
    private static readonly string cloudBlueprints = $@"{BlueprintsRoot}/cloud";
    [AutoPath]
    private static readonly string workshopBlueprints = $@"{BlueprintsRoot}/workshop/temp/Steam";
}
