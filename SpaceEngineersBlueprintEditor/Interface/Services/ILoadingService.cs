namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface ILoadingService : IGlobalService
{
    void Initialize(Grid loadingGrid, TextBlock textBlock);
    void StartLoading(string showText = "Loading...");
    void StopLoading();
}
