using SpaceEngineersBlueprintEditor.Utilities;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// Editor page provide functions edit ship blueprints
/// </summary>
public sealed partial class BlueprintEditorPage : Page
{
    public static BlueprintEditorPage? Current { get; set; }
    public BlueprintEditorPage()
    {
        PageManager.AddOrUpdateCurrentPage(Current = this);
        this.InitializeComponent();
    }
}