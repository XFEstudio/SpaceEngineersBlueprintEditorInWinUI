using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.ViewModels;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// �鿴��ѡ��Ҫ�༭����ͼ��ҳ��
/// </summary>
public sealed partial class BlueprintsViewPage : Page
{
    private object? parameter;

    public object? Parameter
    {
        get { return parameter; }
        set { parameter = value; ViewModel.NavigationParameterService.OnParameterChange(value); }
    }

    public BlueprintsViewPageViewModel ViewModel { get; set; } = new();

    public BlueprintsViewPage? Current { get; set; }
    public BlueprintsViewPage()
    {
        PageManager.AddOrUpdateCurrentPage(Current = this);
        this.InitializeComponent();
    }

    private void GridView_ItemClick(object sender, ItemClickEventArgs e)
    {
        
    }

    private void GridView_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
    {
        
    }
}
