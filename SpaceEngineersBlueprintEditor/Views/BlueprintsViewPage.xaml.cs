using SpaceEngineersBlueprintEditor.Utilities;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// �鿴��ѡ��Ҫ�༭����ͼ��ҳ��
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
