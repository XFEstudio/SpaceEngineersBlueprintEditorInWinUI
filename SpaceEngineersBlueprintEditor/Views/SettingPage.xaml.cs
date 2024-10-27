using SpaceEngineersBlueprintEditor.Utilities;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// App's setting page
/// </summary>
public sealed partial class SettingPage : Page
{
    public SettingPage Current { get; set; }
    public SettingPage()
    {
        PageManager.AddOrUpdateCurrentPage(Current = this);
        this.InitializeComponent();
    }
}
