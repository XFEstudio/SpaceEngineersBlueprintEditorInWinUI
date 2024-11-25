namespace SpaceEngineersBlueprintEditor.Utilities.Addition;

/// <summary>
/// 导航附加属性
/// </summary>
public class NavigationAddition
{
    public static string GetNavigateTo(NavigationViewItem item) => (string)item.GetValue(NavigateToProperty);
    public static void SetNavigateTo(NavigationViewItem item, string value) => item.SetValue(NavigateToProperty, value);

    public static readonly DependencyProperty NavigateToProperty = DependencyProperty.RegisterAttached("NavigateTo", typeof(string), typeof(NavigationAddition), new PropertyMetadata(""));

    public static object GetNavigateParameter(NavigationViewItem item) => item.GetValue(NavigateParameterProperty);
    public static void SetNavigateParameter(NavigationViewItem item, object value) => item.SetValue(NavigateParameterProperty, value);

    public static readonly DependencyProperty NavigateParameterProperty = DependencyProperty.RegisterAttached("NavigateParameter", typeof(object), typeof(NavigationAddition), new PropertyMetadata(null));
}