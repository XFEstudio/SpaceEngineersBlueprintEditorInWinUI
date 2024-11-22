namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface INavigationViewService : IAutoNavigable, IGlobalService
{
    NavigationView? NavigationView { get; }
    IAutoNavigationService NavigationService { get; }
    void Initialize(NavigationView navigationView, Frame frame);
    object? SelectedItem { get; }
    string? Header { get; set; }
    Thickness ContentMargin { get; set; }
    XamlRoot? XamlRoot { get; }
    NavigationViewItem? GetSelectedItem(Type type, object? parameter = null);
}
