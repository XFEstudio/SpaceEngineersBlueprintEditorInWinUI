using Windows.UI.ViewManagement;

namespace SpaceEngineersBlueprintEditor;

/// <summary>
/// Ö÷´°¿Ú
/// </summary>
public sealed partial class MainWindow : Window
{
    public static UISettings UISettings { get; set; } = new UISettings();
    public MainWindow()
    {
        this.InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/Icons/EditorIcon.ico"));
        UISettings.ColorValuesChanged += (_, _) => DispatcherQueue.TryEnqueue(() => AppThemeHelper.ChangeTheme(SystemProfile.Theme));
    }
}
