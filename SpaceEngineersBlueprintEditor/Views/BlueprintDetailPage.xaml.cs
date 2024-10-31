using Microsoft.UI.Xaml.Navigation;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.ViewModels;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// ¿∂ÕºœÍ«È“≥√Ê
/// </summary>
public sealed partial class BlueprintDetailPage : Page
{
    public static BlueprintDetailPage? Current { get; set; }
    public BlueprintInfoViewData? Parameter { get; set; }
    public BlueprintDetailPageViewModel ViewModel { get; set; } = new();
    public BlueprintDetailPage()
    {
        PageManager.AddOrUpdateCurrentPage(Current = this);
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        if (e.Parameter is BlueprintInfoViewData parameter)
        {
            Parameter = parameter;
            ViewModel.NavigationParameterService.OnParameterChange(Parameter);
        }
    }
}
