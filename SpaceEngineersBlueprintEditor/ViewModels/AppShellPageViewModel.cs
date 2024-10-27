using CommunityToolkit.Mvvm.ComponentModel;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Services;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class AppShellPageViewModel : ViewModelBase
{
    [ObservableProperty]
    object? selected;
    [ObservableProperty]
    bool canGoBack;
    public INavigationViewService NavigationViewService { get; set; } = new NavigationViewService();

    public AppShellPageViewModel() => NavigationViewService.NavigationService.Navigated += NavigationService_Navigated;

    private void NavigationService_Navigated(object? sender, Type e)
    {
        CanGoBack = NavigationViewService.NavigationService.CanGoBack;
        if (NavigationViewService.GetSelectedItem(e) is NavigationViewItem item)
            Selected = item;
    }
}
