using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Utilities;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

internal class AutoNavigationService : GlobalServiceBase, IAutoNavigationService
{
    private Frame? frame;
    public bool CanGoBack => frame is not null && frame.CanGoBack;
    public bool CanGoForward => frame is not null && frame.CanGoForward;
    public Frame? Frame
    {
        get => frame;
        set
        {
            if (value is not null)
            {
                value.Navigated += (s, e) => Navigated?.Invoke(s, e);
                if (frame is not null)
                    frame.Navigated -= (s, e) => Navigated?.Invoke(s, e);
                frame = value;
            }
        }
    }

    public event NavigatedEventHandler? Navigated;

    public void GoBack() => frame?.GoBack();

    public void GoForward() => frame?.GoForward();

    public void NavigateTo(string pageType, object? parameter = null, NavigationTransitionInfo? navigationTransitionInfo = null)
    {
        if (PageManager.PageDefinitions.TryGetValue(pageType, out var type))
            NavigateTo(type, parameter, navigationTransitionInfo);
    }

    public void NavigateTo(Type type, object? parameter = null, NavigationTransitionInfo? navigationTransitionInfo = null)
    {
        if (frame?.SourcePageType != type || (frame.Content is Page page && !Equals(page.GetParameter(), parameter)))
            frame?.Navigate(type, parameter, navigationTransitionInfo);
    }

    public void NavigateTo<T>(object? parameter = null, NavigationTransitionInfo? navigationTransitionInfo = null) where T : Page => NavigateTo(typeof(T), parameter, navigationTransitionInfo);
}
