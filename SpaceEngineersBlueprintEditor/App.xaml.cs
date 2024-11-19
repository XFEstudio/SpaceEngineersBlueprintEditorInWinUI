using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.SpaceEngineersCore;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.Views;
using XFEExtension.NetCore.StringExtension;

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
        PageManager.RegisterPage(typeof(MainPage));
        PageManager.RegisterPage(typeof(BlueprintEditPage));
        PageManager.RegisterPage(typeof(BlueprintsViewPage));
        PageManager.RegisterPage(typeof(GameDefinitionsViewPage));
        PageManager.RegisterPage(typeof(BlueprintDetailPage));
        PageManager.RegisterPage(typeof(SettingPage));
        Task.Run(BlueprintsManager.LoadBlueprintsAsync);
        Task.Run(() =>
        {
            try
            {
                if (SystemProfile.GameRootPath.IsNullOrEmpty())
                    SystemProfile.GameRootPath = SpaceEngineersHelper.GetGameRootPath();
                Initializer.Initialize(SpaceEngineersPath.SpaceEngineerContentPath, SpaceEngineersPath.UserGameDataRoot);
                GlobalServiceManager.GetService<IMessageService>()?.ShowMessage("游戏集定义加载完成", "完成", InfoBarSeverity.Success);
            }
            catch (Exception ex)
            {
                GlobalServiceManager.GetService<IMessageService>()?.ShowMessage($"加载定义时发生错误：{ex.Message}", "未能加载定义", InfoBarSeverity.Error);
            }
        });
        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        if (GlobalServiceManager.GetService<IMessageService>() is IMessageService messageService)
        {
            messageService.ShowMessage(e.Message, "错误", InfoBarSeverity.Error);
            e.Handled = true;
        }
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
