using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.ViewModels;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// 查看和选择要编辑的蓝图的页面
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
}
