using SpaceEngineersBlueprintEditor.Model;
using VRage.Game;

namespace SpaceEngineersBlueprintEditor.Utilities.Selector;

/// <summary>
/// 飞船蓝图目标项选择器
/// </summary>
public partial class ShipBlueprintItemTemplateSelector : DataTemplateSelector
{
    /// <summary>
    /// 默认模板项
    /// </summary>
    public DataTemplate? DefaultTemplate { get; set; }
    /// <summary>
    /// 方块模板项
    /// </summary>
    public DataTemplate? CubeItemTemplate { get; set; }
    /// <summary>
    /// 网格模板项
    /// </summary>
    public DataTemplate? GridItemTemplate { get; set; }
    /// <summary>
    /// 对象模板项
    /// </summary>
    public DataTemplate? ObjectItemTemplate { get; set; }
    /// <summary>
    /// 列表模板项
    /// </summary>
    public DataTemplate? EnumerableTemplate { get; set; }
    /// <summary>
    /// 字符串模板项
    /// </summary>
    public DataTemplate? StringValueItemTemplate { get; set; }
    /// <summary>
    /// 数字模板项
    /// </summary>
    public DataTemplate? NumberValueItemTemplate { get; set; }
    /// <summary>
    /// 枚举模板项
    /// </summary>
    public DataTemplate? EnumValueItemTemplate { get; set; }
    /// <summary>
    /// 复合枚举模板项
    /// </summary>
    public DataTemplate? MultiEnumValueItemTemplate { get; set; }
    /// <summary>
    /// 布尔模板项
    /// </summary>
    public DataTemplate? BooleanValueItemTemplate { get; set; }

    ///<inheritdoc/>
    protected override DataTemplate? SelectTemplateCore(object item)
    {
        if (item is TreeViewNode treeViewNode && treeViewNode.Content is BlueprintPropertyViewData blueprintPropertyViewData)
        {
            if (blueprintPropertyViewData.IsBasicType)
            {
                if (blueprintPropertyViewData.IsMultiEnum)
                    return MultiEnumValueItemTemplate;
                else if (blueprintPropertyViewData.Type is not null && blueprintPropertyViewData.Type.IsEnum)
                    return EnumValueItemTemplate;
                else if (blueprintPropertyViewData.Type == typeof(int) || blueprintPropertyViewData.Type == typeof(double) || blueprintPropertyViewData.Type == typeof(float) || blueprintPropertyViewData.Type == typeof(short) || blueprintPropertyViewData.Type == typeof(byte) || blueprintPropertyViewData.Type == typeof(uint) || blueprintPropertyViewData.Type == typeof(ushort))
                    return NumberValueItemTemplate;
                else if (blueprintPropertyViewData.Type == typeof(bool))
                    return BooleanValueItemTemplate;
                else
                    return StringValueItemTemplate;
            }
            else if (blueprintPropertyViewData.IsEnumerable)
                return EnumerableTemplate;
            else if (blueprintPropertyViewData.Type is not null)
            {
                if (blueprintPropertyViewData.Type.IsAssignableTo(typeof(MyObjectBuilder_CubeBlock)))
                    return CubeItemTemplate;
                else if (blueprintPropertyViewData.Type.IsAssignableTo(typeof(MyObjectBuilder_CubeGrid)))
                    return GridItemTemplate;
                else
                    return ObjectItemTemplate;
            }
            else
                return ObjectItemTemplate;
        }
        return DefaultTemplate;
    }
}
