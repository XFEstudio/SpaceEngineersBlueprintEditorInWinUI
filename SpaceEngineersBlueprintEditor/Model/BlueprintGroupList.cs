namespace SpaceEngineersBlueprintEditor.Model;

public partial class BlueprintGroupList(IEnumerable<BlueprintPropertyViewData> collection) : List<BlueprintPropertyViewData>(collection)
{
    public required string GroupName { get; set; }
}
