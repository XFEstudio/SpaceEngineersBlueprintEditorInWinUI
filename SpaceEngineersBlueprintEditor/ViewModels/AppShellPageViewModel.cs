using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Navigation;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class AppShellPageViewModel : ViewModelBase
{
    [ObservableProperty]
    object? selected;
    [ObservableProperty]
    bool canGoBack;
    public INavigationViewService NavigationViewService { get; set; } = new NavigationViewService();
    public IBackgroundImageService BackgroundImageService { get; set; } = new BackgroundImageService();
    public IMessageService MessageService { get; set; } = new MessageService();
    public ILoadingService LoadingService { get; set; } = new LoadingService();

    public AppShellPageViewModel() => NavigationViewService.NavigationService.Navigated += NavigationService_Navigated;

    private void NavigationService_Navigated(object? sender, NavigationEventArgs e) => CanGoBack = NavigationViewService.NavigationService.CanGoBack;
}
