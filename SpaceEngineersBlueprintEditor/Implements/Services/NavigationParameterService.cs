using SpaceEngineersBlueprintEditor.Interface.Services;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

internal class NavigationParameterService<T> : INavigationParameterService<T>
{
    private T? _parameter = default;

    public T? Parameter { get => _parameter; }

    public event EventHandler<T>? ParameterChange;

    public void OnParameterChange(T parameter)
    {
        _parameter = parameter;
        ParameterChange?.Invoke(this, parameter);
    }
}
