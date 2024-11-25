namespace SpaceEngineersBlueprintEditor.Model;

/// <summary>
/// 蓝图编组列表
/// </summary>
/// <param name="collection"></param>
public partial class BlueprintGroupList(IEnumerable<BlueprintPropertyViewData> collection) : List<BlueprintPropertyViewData>(collection)
{
    /// <summary>
    /// 编组名称
    /// </summary>
    public required string GroupName { get; set; }
}
