using SpaceEngineersBlueprintEditor.Utilities;
using XFEExtension.NetCore.AutoConfig;

namespace SpaceEngineersBlueprintEditor.Profiles.CrossVersionProfiles;

public partial class SystemProfile
{
    SystemProfile() => ProfilePath = $@"{AppPath.LocalProfile}\{nameof(SystemProfile)}.xpf";
    [ProfileProperty]
    private ElementTheme theme = ElementTheme.Default;

    static partial void SetThemeProperty(ElementTheme value)
    {
        AppThemeHelper.ChangeTheme(value);
    }
}
