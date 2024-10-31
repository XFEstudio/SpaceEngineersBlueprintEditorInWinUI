using Microsoft.UI.Xaml.Navigation;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.ViewModels;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// 查看和选择要编辑的蓝图的页面
/// </summary>
public sealed partial class BlueprintsViewPage : Page
{
    public string? Parameter { get; set; }

    public BlueprintsViewPageViewModel ViewModel { get; set; } = new();

    public BlueprintsViewPage? Current { get; set; }
    public BlueprintsViewPage()
    {
        PageManager.AddOrUpdateCurrentPage(Current = this);
        this.InitializeComponent();
        NavigationCacheMode = NavigationCacheMode.Enabled;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        if (e.Parameter is string parameter)
        {
            Parameter = parameter;
            ViewModel.NavigationParameterService.OnParameterChange(Parameter);
        }
    }

    private void GridView_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (blueprintGridView.ContainerFromItem(e.ClickedItem) is GridViewItem container)
        {
            blueprintGridView.PrepareConnectedAnimation("ForwardConnectedAnimation", e.ClickedItem, "connectedElement");
        }
    }

    private void GridView_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
    {

    }
}
