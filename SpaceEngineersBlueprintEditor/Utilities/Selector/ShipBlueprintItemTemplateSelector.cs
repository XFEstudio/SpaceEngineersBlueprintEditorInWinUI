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
    /// 值类型模板项
    /// </summary>
    public DataTemplate? ValueItemTemplate { get; set; }

    ///<inheritdoc/>
    protected override DataTemplate? SelectTemplateCore(object item)
    {
        if (item is TreeViewNode treeViewNode && treeViewNode.Content is BlueprintPropertyViewData blueprintPropertyViewData)
        {
            if (blueprintPropertyViewData.IsBasicType)
            {
                return ValueItemTemplate;
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
