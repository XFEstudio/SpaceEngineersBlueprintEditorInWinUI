using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.ViewModels;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// ±‡º≠∆˜µƒ…Ë÷√“≥√Ê
/// </summary>
public sealed partial class SettingPage : Page
{
    public SettingPageViewModel ViewModel { get; set; } = new();
    public static SettingPage? Current { get; set; }
    public SettingPage()
    {
        PageManager.AddOrUpdateCurrentPage(Current = this);
        this.InitializeComponent();
        ViewModel.SettingService.AddComboBox(appThemeComboBox, ProfileHelper.GetEnumProfileSaveFunc<ElementTheme>(), ProfileHelper.GetEnumProfileLoadFuncForComboBox());
        ViewModel.SettingService.AddComboBox(navigationStyleComboBox, ProfileHelper.GetEnumProfileSaveFunc<NavigationViewPaneDisplayMode>(), ProfileHelper.GetEnumProfileLoadFuncForComboBox());
        ViewModel.SettingService.Initialize();
        ViewModel.SettingService.RegisterEvents();
    }
}
