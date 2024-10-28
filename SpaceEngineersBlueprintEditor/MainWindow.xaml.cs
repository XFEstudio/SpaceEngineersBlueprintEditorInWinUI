namespace SpaceEngineersBlueprintEditor;

/// <summary>
/// Ö÷´°¿Ú
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/Icons/EditorIcon.ico"));
    }
}
