namespace SpaceEngineersBlueprintEditor.Interface;

public interface INavigable
{
    void NavigateTo<T>() where T : Page;
    void NavigateTo(Type type);
    void NavigateTo(string pageName);
}
