using System.Reflection;
using XFEExtension.NetCore.AutoPath;

namespace SpaceEngineersBlueprintEditor.Utilities;

/// <summary>
/// 应用程序路径管理
/// </summary>
public partial class AppPath
{
    /// <summary>
    /// 应用程序同步路径
    /// </summary>
    [AutoPath]
    private static readonly string appSynData = @$"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\SpaceEngineersBlueprintsEditor";
    /// <summary>
    /// 应用程序本地路径
    /// </summary>
    [AutoPath]
    private static readonly string appLocalData = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\SpaceEngineersBlueprintsEditor\CrossVersion";
    /// <summary>
    /// 应用程序本地当前版本路径
    /// </summary>
    [AutoPath]
    private static readonly string appLocalVersionData = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\SpaceEngineersBlueprintsEditor\Versions\{(Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString() ?? "UnknownVersion")}";
    /// <summary>
    /// 应用程序缓存路径
    /// </summary>
    [AutoPath]
    private static readonly string appCache = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\Temp\SpaceEngineersBlueprintsEditor";
    /// <summary>
    /// 定义集图片路径
    /// </summary>
    [AutoPath]
    private static readonly string definitionImages = @$"{AppCache}\DefinitionImages";
    /// <summary>
    /// 应用程序本地配置文件路径
    /// </summary>
    [AutoPath]
    private static readonly string localProfile = @$"{AppLocalData}\Profiles";
    /// <summary>
    /// 应用程序本地当前版本配置文件路径
    /// </summary>
    [AutoPath]
    private static readonly string localVersionProfile = $@"{AppLocalVersionData}\Profiles";
    /// <summary>
    /// 应用程序同步配置文件路径
    /// </summary>
    [AutoPath]
    private static readonly string synProfile = $@"{AppSynData}\Profiles";
    /// <summary>
    /// 应用程序缓存路径
    /// </summary>
    [AutoPath]
    private static readonly string cacheProfile = $@"{AppCache}\Profiles";
}
