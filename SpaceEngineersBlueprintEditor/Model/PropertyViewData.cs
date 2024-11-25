namespace SpaceEngineersBlueprintEditor.Model;

/// <summary>
/// 属性视图数据
/// </summary>
public class PropertyViewData
{
    /// <summary>
    /// 属性名称
    /// </summary>
    public required string PropertyName { get; set; }
    /// <summary>
    /// 属性值
    /// </summary>
    public object? Value { get; set; }
    /// <summary>
    /// 是否是值类型
    /// </summary>
    public bool IsValueType { get; set; }
    /// <summary>
    /// 是否是非值类型
    /// </summary>
    public bool IsNotValueType => !IsValueType;
    /// <summary>
    /// 是否有值
    /// </summary>
    public bool HasValue => Value is not null;
    /// <summary>
    /// 是否没有值
    /// </summary>
    public bool HasNoValue => Value is null;
}
