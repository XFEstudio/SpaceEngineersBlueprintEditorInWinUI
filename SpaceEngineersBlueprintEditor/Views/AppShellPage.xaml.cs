using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.ViewModels;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// 编辑器的Shell布局页面
/// </summary>
public sealed partial class AppShellPage : Page
{
    public static AppShellPage? Current { get; set; }
    public AppShellPageViewModel ViewModel { get; set; } = new();
    public AppShellPage()
    {
        PageManager.AddOrUpdateCurrentPage(Current = this);
        this.InitializeComponent();
        App.MainWindow?.SetTitleBar(appTitleBar);
        ViewModel.NavigationViewService.Initialize(navigationView, navigationFrame);
        ViewModel.NavigationViewService.NavigateTo<BlueprintEditPage>();
    }

    private void NavigationView_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        appTitleBar.Margin = new Thickness()
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = appTitleBar.Margin.Top,
            Right = appTitleBar.Margin.Right,
            Bottom = appTitleBar.Margin.Bottom
        };
    }

    private void Page_Loaded(object sender, RoutedEventArgs e) => AppThemeHelper.ChangeTheme(SystemProfile.Theme);
}
