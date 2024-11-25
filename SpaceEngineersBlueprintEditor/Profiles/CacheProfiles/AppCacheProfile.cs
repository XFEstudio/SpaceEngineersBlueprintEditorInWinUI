using SpaceEngineersBlueprintEditor.Utilities;
using XFEExtension.NetCore.AutoConfig;

namespace SpaceEngineersBlueprintEditor.Profiles.CacheProfiles;

/// <summary>
/// 应用程序缓存配置文件
/// </summary>
public partial class AppCacheProfile
{
    /// <summary>
    /// 应用程序缓存配置文件
    /// </summary>
    public AppCacheProfile() => ProfilePath = @$"{AppPath.CacheProfile}\{typeof(AppCacheProfile)}.xpf";
    /// <summary>
    /// 缓存文本
    /// </summary>
    [ProfileProperty]
    private string cacheText = "";
}
