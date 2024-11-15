using Microsoft.UI.Xaml.Media.Animation;
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
        base.OnNavigatedTo(e);
        if (e.Parameter is BlueprintInfoViewData parameter)
        {
            ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardConnectedAnimation")?.TryStart(detailPreviewImage, []);
            Parameter = parameter;
            ViewModel.NavigationParameterService.OnParameterChange(Parameter);
        }
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        if (e.SourcePageType == typeof(BlueprintsViewPage))
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("BackConnectedAnimation", detailPreviewImage);
    }
}
