namespace SpaceEngineersBlueprintEditor.Interface;

public interface ICustomNavigable
{
    void NavigateTo<T>(object? parameter = null) where T : Page;
    void NavigateTo(Type type, object? parameter = null, bool goBack = false);
    void NavigateTo(string pageName, object? parameter = null);
}
