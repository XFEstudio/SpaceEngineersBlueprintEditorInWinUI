namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface INavigationParameterService
{
    object? Parameter { get; }
    event EventHandler<object?> ParameterChange;
    void OnParameterChange(object? parameter);
}
