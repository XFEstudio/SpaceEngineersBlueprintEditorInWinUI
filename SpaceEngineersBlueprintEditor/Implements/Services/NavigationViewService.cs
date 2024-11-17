using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Utilities.Addition;
using System.Diagnostics.CodeAnalysis;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

internal class NavigationViewService : GlobalServiceBase, INavigationViewService
{
    private NavigationView? navigationView;
    private readonly AutoNavigationService navigationService = new();

    public IAutoNavigationService NavigationService => navigationService;
    public object? SelectedItem => navigationView?.SelectedItem;
    public string? Header { get => navigationView?.Header.ToString(); set { if (navigationView is not null) navigationView.Header = value; } }

    [MemberNotNull(nameof(navigationView))]
    public void Initialize(NavigationView view, Frame frame)
    {
        navigationService.Frame = frame;
        navigationView = view;
        navigationView.ItemInvoked += NavigationView_ItemInvoked;
        navigationView.BackRequested += NavigationView_BackRequested;
        navigationService.Navigated += NavigationService_Navigated;
    }

    private void NavigationService_Navigated(object? sender, NavigationEventArgs e)
    {
        if (navigationView is not null && GetSelectedItem(e.SourcePageType, e.Parameter) is NavigationViewItem navigationViewItem)
        {
            navigationView.SelectedItem = navigationViewItem;
            Header = navigationViewItem.Content.ToString() ?? string.Empty;
        }
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

    public NavigationViewItem? GetSelectedItem(Type type, object? parameter = null) => navigationView is null ? null : GetSelectedItem(navigationView.MenuItems, navigationView.FooterMenuItems, type, parameter);

    private static NavigationViewItem? GetSelectedItem(IEnumerable<object> menuItems, IEnumerable<object> footerMenuItems, Type pageType, object? parameter)
    {
        var footerResult = GetSelectedItem(footerMenuItems, pageType, parameter);
        if (footerResult is null)
            return GetSelectedItem(menuItems, pageType, parameter);
        return footerResult;
    }

    private static NavigationViewItem? GetSelectedItem(IEnumerable<object> menuItems, Type pageType, object? parameter)
    {
        foreach (var item in menuItems.OfType<NavigationViewItem>())
        {
            if (item.GetNavigateTo() is string pageName && pageName == pageType.FullName)
            {
                var itemParameter = item.GetNavigationParameter();
                if (parameter is string && Equals(parameter, itemParameter) || parameter == itemParameter || itemParameter is null)
                    return item;
            }
            var selectedChild = GetSelectedItem(item.MenuItems, pageType, parameter);
            if (selectedChild != null)
                return selectedChild;
        }
        return null;
    }

    public void NavigateTo(string pageType, object? parameter = null, NavigationTransitionInfo? navigationTransitionInfo = null) => navigationService.NavigateTo(pageType, parameter, navigationTransitionInfo);

    public void NavigateTo(Type type, object? parameter = null, NavigationTransitionInfo? navigationTransitionInfo = null) => navigationService.NavigateTo(type, parameter, navigationTransitionInfo);

    public void NavigateTo<T>(object? parameter = null, NavigationTransitionInfo? navigationTransitionInfo = null) where T : Page => navigationService.NavigateTo<T>(parameter, navigationTransitionInfo);
}
