using Microsoft.UI.Xaml.Media.Animation;

namespace SpaceEngineersBlueprintEditor.Interface;

public interface IAutoNavigable
{
    void NavigateTo(string pageType, object? parameter = null, NavigationTransitionInfo? navigationTransitionInfo = null);
    void NavigateTo(Type type, object? parameter = null, NavigationTransitionInfo? navigationTransitionInfo = null);
    void NavigateTo<T>(object? parameter = null, NavigationTransitionInfo? navigationTransitionInfo = null) where T : Page;
}