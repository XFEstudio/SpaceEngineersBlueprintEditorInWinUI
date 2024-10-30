using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Utilities.Addition;
using System.Diagnostics.CodeAnalysis;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

internal class NavigationViewService : GlobalServiceBase, INavigationViewService
{
    private NavigationView? navigationView;
    private readonly INavigationService navigationService = new NavigationService();
    public INavigationService NavigationService => navigationService;

    public object? SelectedItem => navigationView?.SelectedItem;

    [MemberNotNull(nameof(navigationView))]
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
        if (args.InvokedItemContainer.GetValue(NavigationAddition.NavigateToProperty) is string targetUrl)
            NavigateTo(targetUrl, args.InvokedItemContainer.GetValue(NavigationAddition.NavigateParameterProperty) is string parameter ? parameter : null);
    }

    public void NavigateTo<T>(object? parameter = null) where T : Page => NavigationService.NavigateTo<T>(parameter);

    public void NavigateTo(Type type, object? parameter = null, bool goBack = false) => NavigationService.NavigateTo(type, parameter, goBack);

    public void NavigateTo(string pageName, object? parameter = null) => NavigationService.NavigateTo(pageName, parameter);

    public NavigationViewItem? GetSelectedItem(Type type) => navigationView is null ? null : GetSelectedItem(navigationView.MenuItems, navigationView.FooterMenuItems, type);
    private NavigationViewItem? GetSelectedItem(IEnumerable<object> menuItems, IEnumerable<object> footerMenuItems, Type pageType)
    {
        var footerResult = GetSelectedItem(footerMenuItems, pageType);
        if (footerResult is null)
            return GetSelectedItem(menuItems, pageType);
        return footerResult;
    }
    private NavigationViewItem? GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
    {
        foreach (var item in menuItems.OfType<NavigationViewItem>())
        {
            if (item.GetNavigateTo() is string pageName && pageName == pageType.FullName)
            {
                var parameter = NavigationService.NavigationStack.Last().Item2;
                var itemParameter = item.GetNavigationParameter();
                if (parameter is string && Equals(parameter, itemParameter) || parameter == itemParameter)
                    return item;
            }
            var selectedChild = GetSelectedItem(item.MenuItems, pageType);
            if (selectedChild != null)
                return selectedChild;
        }
        return null;
    }
}
