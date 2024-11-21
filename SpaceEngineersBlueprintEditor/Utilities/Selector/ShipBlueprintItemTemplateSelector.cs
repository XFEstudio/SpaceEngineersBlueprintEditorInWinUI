using SpaceEngineersBlueprintEditor.Model;
using VRage.Game;

namespace SpaceEngineersBlueprintEditor.Utilities.Selector;

public partial class ShipBlueprintItemTemplateSelector : DataTemplateSelector
{
    public DataTemplate? DefaultTemplate { get; set; }
    public DataTemplate? CubeItemTemplate { get; set; }
    public DataTemplate? GridItemTemplate { get; set; }
    public DataTemplate? ObjectItemTemplate { get; set; }
    public DataTemplate? EnumerableTemplate { get; set; }
    public DataTemplate? StringValueItemTemplate { get; set; }
    public DataTemplate? NumberValueItemTemplate { get; set; }
    public DataTemplate? EnumValueItemTemplate { get; set; }
    public DataTemplate? MultiEnumValueItemTemplate { get; set; }
    public DataTemplate? BooleanValueItemTemplate { get; set; }

    protected override DataTemplate? SelectTemplateCore(object item)
    {
        if (item is TreeViewNode treeViewNode && treeViewNode.Content is BlueprintPropertyViewData blueprintPropertyViewData)
        {
            if (blueprintPropertyViewData.IsBasicType)
            {
                if (blueprintPropertyViewData.Type == typeof(string))
                    return StringValueItemTemplate;
                if (blueprintPropertyViewData.Type == typeof(bool))
                    return BooleanValueItemTemplate;
                else if (blueprintPropertyViewData.IsMultiEnum)
                    return MultiEnumValueItemTemplate;
                else if (blueprintPropertyViewData.Type is not null && blueprintPropertyViewData.Type.IsEnum)
                    return EnumValueItemTemplate;
                else
                    return NumberValueItemTemplate;
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
