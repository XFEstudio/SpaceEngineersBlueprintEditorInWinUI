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
        NavigationCacheMode = NavigationCacheMode.Enabled;
        this.InitializeComponent();
        ViewModel.NavigationParameterService.Initialize(this);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        ViewModel.NavigationParameterService.OnParameterChange(e.Parameter);
        if (e.Parameter is BlueprintInfoViewData parameter)
        {
            ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardConnectedAnimation")?.TryStart(detailPreviewImage, []);
            detailPreviewImage.Source = parameter.BlueprintImage;
        }
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        if (e.SourcePageType == typeof(BlueprintsViewPage))
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("BackConnectedAnimation", detailPreviewImage);
        if (e.SourcePageType != typeof(BlueprintEditPage))
            ViewModel.BackgroundImageService?.ResetBackground();
    }
}
