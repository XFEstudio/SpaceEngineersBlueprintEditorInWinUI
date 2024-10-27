namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface INavigationViewService : INavigable
{
    INavigationService NavigationService { get; }
    void Initialize(NavigationView navigationView, Frame frame);
    object? SelectedItem { get; }
    NavigationViewItem? GetSelectedItem(Type type);
}
