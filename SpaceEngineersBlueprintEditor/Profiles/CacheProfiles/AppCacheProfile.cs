using SpaceEngineersBlueprintEditor.Utilities;
using XFEExtension.NetCore.AutoConfig;

namespace SpaceEngineersBlueprintEditor.Profiles.CacheProfiles;

public partial class AppCacheProfile
{
    public AppCacheProfile() => ProfilePath = @$"{AppPath.CacheProfile}\{typeof(AppCacheProfile)}.xpf";
    [ProfileProperty]
    private string cacheText = "";
}
