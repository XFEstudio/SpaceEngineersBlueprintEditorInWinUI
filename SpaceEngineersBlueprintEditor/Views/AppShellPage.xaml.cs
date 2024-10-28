using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.ViewModels;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// App Main Shell Page
/// </summary>
public sealed partial class AppShellPage : Page
{
    public static AppShellPage? Current { get; set; }
    public AppShellPageViewModel ViewModel { get; set; }
    public AppShellPage()
    {
        PageManager.AddOrUpdateCurrentPage(Current = this);
        DataContext = ViewModel = new AppShellPageViewModel();
        this.InitializeComponent();
        App.MainWindow?.SetTitleBar(appTitleBar);
        ViewModel.NavigationViewService.Initialize(navigationView, navigationFrame);
        ViewModel.NavigationViewService.NavigateTo<BlueprintEditorPage>();
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
}
