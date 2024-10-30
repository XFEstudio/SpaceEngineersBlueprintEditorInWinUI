namespace SpaceEngineersBlueprintEditor.Interface;

public interface INavigable
{
    void NavigateTo<T>(object? parameter = null) where T : Page;
    void NavigateTo(Type type, object? parameter = null, bool goBack = false);
    void NavigateTo(string pageName, object? parameter = null);
}
