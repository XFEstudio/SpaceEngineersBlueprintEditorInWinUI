namespace SpaceEngineersBlueprintEditor.Model;

public class PropertyViewData
{
    public required string PropertyName { get; set; }
    public object? Value { get; set; }
    public bool IsValueType { get; set; }
    public bool IsNotValueType => !IsValueType;
}
