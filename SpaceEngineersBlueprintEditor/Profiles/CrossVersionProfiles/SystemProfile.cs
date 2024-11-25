using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Utilities;
using XFEExtension.NetCore.AutoConfig;

namespace SpaceEngineersBlueprintEditor.Profiles.CrossVersionProfiles;

/// <summary>
/// 系统配置文件
/// </summary>
public partial class SystemProfile
{
    /// <summary>
    /// 系统配置文件
    /// </summary>
    SystemProfile() => ProfilePath = $@"{AppPath.LocalProfile}\{nameof(SystemProfile)}.xpf";
    /// <summary>
    /// 游戏根目录
    /// </summary>
    [ProfileProperty]
    private string gameRootPath = "";
    /// <summary>
    /// 导航栏样式
    /// </summary>
    [ProfileProperty]
    private NavigationViewPaneDisplayMode navigationStyle = NavigationViewPaneDisplayMode.Left;
    /// <summary>
    /// 应用程序主题样式
    /// </summary>
    [ProfileProperty]
    private ElementTheme theme = ElementTheme.Default;

    static partial void SetThemeProperty(ElementTheme value) => AppThemeHelper.ChangeTheme(value);
    static partial void SetNavigationStyleProperty(NavigationViewPaneDisplayMode value)
    {
        if (GlobalServiceManager.GetService<INavigationViewService>() is INavigationViewService navigationViewService && navigationViewService.NavigationView is not null)
            navigationViewService.NavigationView.PaneDisplayMode = value;
    }
}
