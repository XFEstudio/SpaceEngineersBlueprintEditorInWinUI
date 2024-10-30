using SpaceEngineersBlueprintEditor.Interface.Services;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

internal class NavigationParameterService : INavigationParameterService
{
    private object? _parameter;

    public object? Parameter { get => _parameter; }

    public event EventHandler<object?>? ParameterChange;

    public void OnParameterChange(object? parameter)
    {
        _parameter = parameter;
        ParameterChange?.Invoke(this, parameter);
    }
}
