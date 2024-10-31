using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.ViewModels;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// �༭����Shell����ҳ��
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
        ViewModel.NavigationViewService.NavigateTo<MainPage>();
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
