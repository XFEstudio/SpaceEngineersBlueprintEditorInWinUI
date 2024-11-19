using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Utilities;
using XFEExtension.NetCore.AutoConfig;

namespace SpaceEngineersBlueprintEditor.Profiles.CrossVersionProfiles;

public partial class SystemProfile
{
    SystemProfile() => ProfilePath = $@"{AppPath.LocalProfile}\{nameof(SystemProfile)}.xpf";
    [ProfileProperty]
    private string gameRootPath = "";
    [ProfileProperty]
    private NavigationViewPaneDisplayMode navigationStyle = NavigationViewPaneDisplayMode.Left;
    [ProfileProperty]
    private ElementTheme theme = ElementTheme.Default;

    static partial void SetThemeProperty(ElementTheme value) => AppThemeHelper.ChangeTheme(value);
    static partial void SetNavigationStyleProperty(NavigationViewPaneDisplayMode value)
    {
        if (GlobalServiceManager.GetService<INavigationViewService>() is INavigationViewService navigationViewService && navigationViewService.NavigationView is not null)
            navigationViewService.NavigationView.PaneDisplayMode = value;
    }
}
