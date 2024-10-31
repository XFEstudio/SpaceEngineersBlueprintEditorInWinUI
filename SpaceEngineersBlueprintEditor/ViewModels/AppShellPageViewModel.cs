using CommunityToolkit.Mvvm.ComponentModel;
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
    public IMessageService MessageService { get; set; } = new MessageService();

    public AppShellPageViewModel() => NavigationViewService.NavigationService.Navigated += NavigationService_Navigated;

    private void NavigationService_Navigated(object? sender, Type e)
    {
        CanGoBack = NavigationViewService.NavigationService.CanGoBack;
        if (NavigationViewService.GetSelectedItem(e) is NavigationViewItem item)
            Selected = item;
    }
}
