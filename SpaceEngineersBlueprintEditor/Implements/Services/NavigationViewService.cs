using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Utilities;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

internal class NavigationViewService : INavigationViewService
{
    private NavigationView? navigationView;
    private readonly INavigationService navigationService = new NavigationService();
    public INavigationService NavigationService => navigationService;

    public object? SelectedItem => navigationView?.SelectedItem;

    public void Initialize(NavigationView view, Frame frame)
    {
        navigationService.Frame = frame;
        navigationView = view;
        navigationView.ItemInvoked += NavigationView_ItemInvoked;
        navigationView.BackRequested += NavigationView_BackRequested;
        navigationService.Navigated += NavigationService_Navigated;
    }

    private void NavigationService_Navigated(object? sender, Type e)
    {
        if (navigationView is not null)
            navigationView.SelectedItem = GetSelectedItem(e);
    }

    private void NavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        if (navigationService.CanGoBack)
            navigationService.GoBack();
    }

    private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer is ContentControl contentControl && contentControl.Tag is string tag)
            NavigateTo(tag);
    }

    public void NavigateTo<T>() where T : Page => NavigationService.NavigateTo<T>();

    public void NavigateTo(Type type) => NavigationService.NavigateTo(type);

    public void NavigateTo(string pageName) => NavigationService.NavigateTo(pageName);

    public NavigationViewItem? GetSelectedItem(Type type) => navigationView is null ? null : GetSelectedItem(navigationView.MenuItems, navigationView.FooterMenuItems, type);
    private static NavigationViewItem? GetSelectedItem(IEnumerable<object> menuItems, IEnumerable<object> footerMenuItems, Type pageType)
    {
        var footerResult = GetSelectedItem(footerMenuItems, pageType);
        if (footerResult is null)
            return GetSelectedItem(menuItems, pageType);
        return footerResult;
    }
    private static NavigationViewItem? GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
    {
        foreach (var item in menuItems.OfType<NavigationViewItem>())
        {
            if (item.Tag is string tag && PageManager.PageDefinitions.TryGetValue(tag, out var type) && type == pageType)
                return item;
            var selectedChild = GetSelectedItem(item.MenuItems, pageType);
            if (selectedChild != null)
                return selectedChild;
        }
        return null;
    }
}
