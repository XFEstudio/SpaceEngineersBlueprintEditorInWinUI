using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.Views;

namespace SpaceEngineersBlueprintEditor;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    public static MainWindow MainWindow { get; set; } = new MainWindow();
    public App()
    {
        this.InitializeComponent();
        PageManager.RegisterPage(typeof(AppShellPage));
        PageManager.RegisterPage(typeof(BlueprintEditPage));
        PageManager.RegisterPage(typeof(BlueprintsViewPage));
        PageManager.RegisterPage(typeof(SettingPage));
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        MainWindow.Content = new AppShellPage();
        MainWindow.Activate();
    }
}
