using SpaceEngineersBlueprintEditor.Interface.Services;
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
        ViewModel.MessageService.Initialize(messageStackPanel, DispatcherQueue);
        ViewModel.LoadingService.Initialize(loadingGrid, loadingTextBlock);
        ViewModel.BackgroundImageService.Initialize(this, mainGrid);
        ViewModel.NavigationViewService.NavigateTo<MainPage>();
    }

    private void NavigationView_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        switch (sender.PaneDisplayMode)
        {
            case NavigationViewPaneDisplayMode.Auto:
                navigationView.Margin = new();
                appTitleBar.Margin = new()
                {
                    Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
                    Top = appTitleBar.Margin.Top,
                    Right = appTitleBar.Margin.Right,
                    Bottom = appTitleBar.Margin.Bottom
                };
                break;
            case NavigationViewPaneDisplayMode.Left:
                navigationView.Margin = new();
                appTitleBar.Margin = new()
                {
                    Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
                    Top = appTitleBar.Margin.Top,
                    Right = appTitleBar.Margin.Right,
                    Bottom = appTitleBar.Margin.Bottom
                };
                break;
            case NavigationViewPaneDisplayMode.Top:
                navigationView.Margin = new(0, 48, 0, 0);
                appTitleBar.Margin = new(16, 0, 0, 0);
                break;
            case NavigationViewPaneDisplayMode.LeftCompact:
                navigationView.Margin = new();
                appTitleBar.Margin = new()
                {
                    Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
                    Top = appTitleBar.Margin.Top,
                    Right = appTitleBar.Margin.Right,
                    Bottom = appTitleBar.Margin.Bottom
                };
                break;
            case NavigationViewPaneDisplayMode.LeftMinimal:
                navigationView.Margin = new();
                appTitleBar.Margin = new()
                {
                    Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
                    Top = appTitleBar.Margin.Top,
                    Right = appTitleBar.Margin.Right,
                    Bottom = appTitleBar.Margin.Bottom
                };
                break;
            default:
                break;
        }
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        ViewModel.MessageService.ShowMessage("游戏定义集加载中...", "正在加载");
        AppThemeHelper.ChangeTheme(SystemProfile.Theme);
        navigationView.PaneDisplayMode = SystemProfile.NavigationStyle;
        SpaceEngineersHelper.LoadComplete += (sender, e) => ViewModel.MessageService?.ShowMessage("游戏集定义加载完成", "完成", InfoBarSeverity.Success); ;
        await SpaceEngineersHelper.LoadDefinitionViewDataListAsync();
    }

    private void NavigationView_PaneOpening(NavigationView sender, object args)
    {
        rightPanelGrid.TranslationTransition = new();
        rightPanelGrid.Translation = new((float)(sender.OpenPaneLength / 2 - sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 1f : 0.5f)) + rightPanelGrid.Translation.X, 0, 0);
    }

    private void NavigationView_PaneOpened(NavigationView sender, object args)
    {
        rightPanelGrid.TranslationTransition = null;
        rightPanelGrid.Translation = new();
        rightPanelGrid.Margin = new()
        {
            Left = sender.OpenPaneLength,
            Top = 50
        };
    }

    private void NavigationView_PaneClosing(NavigationView sender, NavigationViewPaneClosingEventArgs args)
    {
        rightPanelGrid.TranslationTransition = new();
        rightPanelGrid.Translation = new((float)(sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 1f : 0.5f) - sender.OpenPaneLength / 2) + rightPanelGrid.Translation.X, 0, 0);
    }

    private void NavigationView_PaneClosed(NavigationView sender, object args)
    {
        rightPanelGrid.TranslationTransition = null;
        rightPanelGrid.Translation = new();
        rightPanelGrid.Margin = new()
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = 50
        };
    }
}
