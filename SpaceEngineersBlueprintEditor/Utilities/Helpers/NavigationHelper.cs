using SpaceEngineersBlueprintEditor.Utilities.Addition;

namespace SpaceEngineersBlueprintEditor.Utilities.Helpers;

public static class NavigationHelper
{
    public static void SetParameter(this Page page, object? parameter) => page.GetType().GetProperty("Parameter")?.SetValue(page, parameter);
    public static object? GetParameter(this Page page) => page.GetType().GetProperty("Parameter")?.GetValue(page);
    public static string? GetNavigateTo(this NavigationViewItem navigationViewItem) => navigationViewItem.GetValue(NavigationAddition.NavigateToProperty).ToString();
    public static object? GetNavigationParameter(this NavigationViewItem navigationViewItem) => navigationViewItem.GetValue(NavigationAddition.NavigateParameterProperty);
}
