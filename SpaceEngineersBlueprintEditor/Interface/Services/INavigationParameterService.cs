namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface INavigationParameterService<T>
{
    bool SameAsLast { get; }
    Page? Page { get; }
    T? Parameter { get; set; }
    event EventHandler<T?> ParameterChange;
    void Initialize(Page page);
    void OnParameterChange(object? parameter);
}
