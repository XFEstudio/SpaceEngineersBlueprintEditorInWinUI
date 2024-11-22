using SpaceEngineersBlueprintEditor.Interface.Services;
using System.Diagnostics.CodeAnalysis;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

internal class NavigationParameterService<T> : INavigationParameterService<T>
{
    private bool sameAsLast;
    private T? currentParameter;
    private Page? _page;
    public T? Parameter
    {
        get => _page is not null ? NavigationHelper.GetParameter(_page) is T t ? t : default : default;
        set
        {
            if (_page is not null)
                NavigationHelper.SetParameter(_page, value);
        }
    }

    public Page? Page => _page;

    public bool SameAsLast => sameAsLast;

    public event EventHandler<T?>? ParameterChange;

    [MemberNotNull(nameof(_page))]
    public void Initialize(Page page) => _page = page;

    public void OnParameterChange(object? parameter)
    {
        if (parameter is T parameterValue)
            Parameter = parameterValue;
        else
            Parameter = default;
        sameAsLast = Parameter is null ? currentParameter is null : Parameter.Equals(currentParameter);
        if (!sameAsLast)
            currentParameter = Parameter;
        ParameterChange?.Invoke(this, Parameter);
    }
}
