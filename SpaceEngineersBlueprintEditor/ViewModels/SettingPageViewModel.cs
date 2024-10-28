using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using System;
using Windows.System;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class SettingPageViewModel : ViewModelBase
{
    [ObservableProperty]
    string currentVersion = "";
    [ObservableProperty]
    object? appThemeSelectedItem;

    public ISettingService SettingService { get; } = new SettingService();

    public SettingPageViewModel()
    {
        if (System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version is Version version)
        {
            CurrentVersion = version.ToString();
        }
    }

    [RelayCommand]
    static async Task LinkToGithubRepo()
    {
        await Launcher.LaunchUriAsync(new Uri("https://github.com/XFEstudio/SpaceEngineersBlueprintEditorInWinUI"));
    }
}
