using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.ViewModels;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// App's setting page
/// </summary>
public sealed partial class SettingPage : Page
{
    public SettingPageViewModel ViewModel { get; set; }
    public SettingPage? Current { get; set; }
    public SettingPage()
    {
        PageManager.AddOrUpdateCurrentPage(Current = this);
        ViewModel = new SettingPageViewModel();
        this.InitializeComponent();
    }
}
