namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface INavigationViewService : IAutoNavigable
{
    IAutoNavigationService NavigationService { get; }
    void Initialize(NavigationView navigationView, Frame frame);
    object? SelectedItem { get; }
    NavigationViewItem? GetSelectedItem(Type type, object? parameter = null);
}
