using SpaceEngineersBlueprintEditor.Utilities;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// 查看和选择要编辑的蓝图的页面
/// </summary>
public sealed partial class BlueprintsViewPage : Page
{
    public BlueprintsViewPage? Current { get; set; }
    public BlueprintsViewPage()
    {
        PageManager.AddOrUpdateCurrentPage(Current = this);
        this.InitializeComponent();
    }
}
