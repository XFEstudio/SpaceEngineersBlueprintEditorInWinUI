using System.Reflection;
using XFEExtension.NetCore.AutoPath;

namespace SpaceEngineersBlueprintEditor.Utilities;

public partial class AppPath
{
    [AutoPath]
    private static readonly string appSynData = @$"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\SpaceEngineersBlueprintsEditor";
    [AutoPath]
    private static readonly string appLocalData = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\SpaceEngineersBlueprintsEditor\CrossVersion";
    [AutoPath]
    private static readonly string appLocalVersionData = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\SpaceEngineersBlueprintsEditor\Versions\{(Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString() ?? "UnknownVersion")}";
    [AutoPath]
    private static readonly string appCache = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\Temp\SpaceEngineersBlueprintsEditor";
    [AutoPath]
    private static readonly string localProfile = @$"{AppLocalData}\Profiles";
    [AutoPath]
    private static readonly string localVersionProfile = $@"{AppLocalVersionData}\Profiles";
    [AutoPath]
    private static readonly string synProfile = $@"{AppSynData}\Profiles";
    [AutoPath]
    private static readonly string cacheProfile = $@"{AppCache}\Profiles";
}
