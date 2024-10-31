namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface INavigationService : IGlobalService
{
    bool CanGoBack { get; }
    bool CanGoForward { get; }
    Frame? Frame { get; set; }
    void GoBack();
}
