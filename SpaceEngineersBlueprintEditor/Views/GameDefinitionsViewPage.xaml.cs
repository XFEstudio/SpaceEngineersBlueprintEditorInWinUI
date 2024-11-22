using Microsoft.UI.Xaml.Navigation;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.ViewModels;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// 游戏定义集浏览页面
/// </summary>
public sealed partial class GameDefinitionsViewPage : Page
{
    public string? Parameter { get; set; }
    public static GameDefinitionsViewPage? Current { get; set; }
    public GameDefinitionsViewPageViewModel ViewModel { get; set; } = new();
    public GameDefinitionsViewPage()
    {
        PageManager.AddOrUpdateCurrentPage(Current = this);
        this.InitializeComponent();
        ViewModel.DefinitionPropertiesDisplayService.Initialize(this, definitionsItemView);
        ViewModel.NavigationParameterService.Initialize(this);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        ViewModel.NavigationParameterService.OnParameterChange(e.Parameter);
    }
}
