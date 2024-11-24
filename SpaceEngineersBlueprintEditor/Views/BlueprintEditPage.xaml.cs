using Microsoft.UI.Xaml.Navigation;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.ViewModels;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// À¶Í¼±à¼­Ò³Ãæ
/// </summary>
public sealed partial class BlueprintEditPage : Page
{
    public BlueprintModel? Parameter { get; set; }
    public static BlueprintEditPage? Current { get; set; }
    public BlueprintEditPageViewModel ViewModel { get; set; } = new();
    public BlueprintEditPage()
    {
        PageManager.AddOrUpdateCurrentPage(Current = this);
        this.InitializeComponent();
        NavigationCacheMode = NavigationCacheMode.Required;
        ViewModel.NavigationParameterService.Initialize(this);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (ViewModel.NavigationViewService is not null)
        {
            ViewModel.NavigationViewService.Header = null;
            ViewModel.NavigationViewService.ContentMargin = new();
        }
        ViewModel.NavigationParameterService.OnParameterChange(e.Parameter);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        if (ViewModel.NavigationViewService is not null)
            ViewModel.NavigationViewService.ContentMargin = new(56, 24, 56, 0);
        ViewModel.BackgroundImageService?.ResetBackground();
    }

    private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
    {
        ViewModel.TabViewItems.Remove(args.Tab);
        if (ViewModel.TabViewItems.Count == 0)
            ViewModel.CreateTabViewItem(null);
    }
}