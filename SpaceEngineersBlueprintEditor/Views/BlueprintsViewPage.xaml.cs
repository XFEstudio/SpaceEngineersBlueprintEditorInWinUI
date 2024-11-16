using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.ViewModels;
using Windows.Foundation.Metadata;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// 查看和选择要编辑的蓝图的页面
/// </summary>
public sealed partial class BlueprintsViewPage : Page
{
    public string? Parameter { get; set; }
    public BlueprintInfoViewData? CurrentBlueprintInfoViewData { get; set; }
    public BlueprintsViewPageViewModel ViewModel { get; set; } = new();
    public static BlueprintsViewPage? Current { get; set; }

    public BlueprintsViewPage()
    {
        PageManager.AddOrUpdateCurrentPage(Current = this);
        this.InitializeComponent();
        NavigationCacheMode = NavigationCacheMode.Enabled;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (e.Parameter is string parameter && parameter != Parameter)
        {
            Parameter = parameter;
            ViewModel.NavigationParameterService.OnParameterChange(Parameter);
        }
    }

    private void GridView_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is BlueprintInfoViewData blueprintInfoViewData)
        {
            CurrentBlueprintInfoViewData = blueprintInfoViewData;
            ViewModel.NavigationParameterService.OnParameterChange(blueprintInfoViewData);
            blueprintGridView.PrepareConnectedAnimation("ForwardConnectedAnimation", e.ClickedItem, "connectedElement");
            Frame.Navigate(typeof(BlueprintDetailPage), e.ClickedItem, new SuppressNavigationTransitionInfo());
        }
    }

    private void Grid_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
    {
        if (sender is Grid grid && grid.DataContext is BlueprintInfoViewData blueprintInfoViewData)
        {
            CurrentBlueprintInfoViewData = blueprintInfoViewData;
            ViewModel.NavigationParameterService.OnParameterChange(blueprintInfoViewData);
            commandBarFlyout.ShowAt(grid);
        }
    }

    private async void BlueprintGridView_Loaded(object sender, RoutedEventArgs e)
    {
        if (CurrentBlueprintInfoViewData is not null)
        {
            blueprintGridView.ScrollIntoView(CurrentBlueprintInfoViewData, ScrollIntoViewAlignment.Default);
            blueprintGridView.UpdateLayout();
            var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackConnectedAnimation");
            if (animation is not null && ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
            {
                animation.Configuration = new DirectConnectedAnimationConfiguration();
                await blueprintGridView.TryStartConnectedAnimationAsync(animation, CurrentBlueprintInfoViewData, "connectedElement");
            }
            blueprintGridView.Focus(FocusState.Programmatic);
        }
    }
}
