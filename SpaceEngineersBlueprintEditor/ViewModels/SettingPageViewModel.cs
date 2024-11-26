using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.Views;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using XFEExtension.NetCore.FileExtension;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class SettingPageViewModel : ViewModelBase
{
    [ObservableProperty]
    string currentVersion = "";
    [ObservableProperty]
    string spaceEngineersRootPath = "";
    [ObservableProperty]
    string appCachePath = "";
    [ObservableProperty]
    string appCacheSize = "";
    public ISettingService SettingService { get; } = new SettingService();

    public SettingPageViewModel()
    {
        SpaceEngineersRootPath = SystemProfile.GameRootPath;
        AppCachePath = AppPath.AppCache;
        if (System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version is Version version)
        {
            CurrentVersion = version.ToString(3);
        }
        AppCacheSize = FileHelper.GetDirectorySize(new(AppPath.AppCache)).FileSize();
    }

    private static async Task<(ContentDialog, StorageFolder)?> UserChoseAndGetDialog(string originalPath)
    {
        var openPicker = new FolderPicker();
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow));
        openPicker.ViewMode = PickerViewMode.List;
        var folder = await openPicker.PickSingleFolderAsync();
        if (folder is not null && SettingPage.Current is not null)
        {
            var contentDialog = new ContentDialog
            {
                DefaultButton = ContentDialogButton.Primary,
                IsPrimaryButtonEnabled = true,
                Content = $"{"ChangePath_FromPath".GetLocalized()}: {originalPath}\n{"ChangePath_ToPath".GetLocalized()}: {folder.Path}\n\n{"ChangePath_Ensure".GetLocalized()}",
                PrimaryButtonText = "Confirm".GetLocalized(),
                IsSecondaryButtonEnabled = true,
                SecondaryButtonText = "Cancel".GetLocalized(),
                XamlRoot = SettingPage.Current.Content.XamlRoot
            };
            return (contentDialog, folder);
        }
        return null;
    }

    [RelayCommand]
    static async Task LinkToGithubRepo() => await Launcher.LaunchUriAsync(new Uri("https://github.com/XFEstudio/SpaceEngineersBlueprintEditorInWinUI"));

    [RelayCommand]
    static void OpenPath(string originalPath) => Process.Start("explorer.exe", originalPath);

    [RelayCommand]
    async Task ClearCache()
    {
        if (SettingPage.Current is not null)
        {
            var contentDialog = new ContentDialog
            {
                DefaultButton = ContentDialogButton.Primary,
                IsPrimaryButtonEnabled = true,
                Content = "ClearCache_Ensure".GetLocalized(),
                PrimaryButtonText = "Confirm".GetLocalized(),
                IsSecondaryButtonEnabled = true,
                SecondaryButtonText = "Cancel".GetLocalized(),
                XamlRoot = SettingPage.Current.Content.XamlRoot
            };
            if (await contentDialog.ShowAsync() == ContentDialogResult.Primary)
            {
                Directory.Delete(AppPath.AppCache, true);
                AppCacheSize = FileHelper.GetDirectorySize(new(AppPath.AppCache)).FileSize();
            }
        }
    }

    [RelayCommand]
    static async Task ChangeGameRootPath()
    {
        if (await UserChoseAndGetDialog(SystemProfile.GameRootPath) is (ContentDialog, StorageFolder) result && await result.Item1.ShowAsync() == ContentDialogResult.Primary)
            SystemProfile.GameRootPath = result.Item2.Path;
    }
}
