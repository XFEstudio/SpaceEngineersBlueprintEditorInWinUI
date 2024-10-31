namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface INavigationParameterService<T>
{
    T? Parameter { get; }
    event EventHandler<T> ParameterChange;
    void OnParameterChange(T parameter);
}
