namespace SpaceEngineersBlueprintEditor.Interface;

public interface INavigable
{
    void NavigateTo<T>(object? parameter = null) where T : Page;
    void NavigateTo(Type type, object? parameter = null);
    void NavigateTo(string pageName, object? parameter = null);
}
