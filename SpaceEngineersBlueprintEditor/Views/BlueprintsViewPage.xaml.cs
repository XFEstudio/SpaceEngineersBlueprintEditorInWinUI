using SpaceEngineersBlueprintEditor.Utilities;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// View and select blueprints to edit
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
