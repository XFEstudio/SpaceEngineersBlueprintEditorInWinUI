using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using Windows.System;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class SettingPageViewModel : ViewModelBase
{
    [ObservableProperty]
    string currentVersion = "";

    public SettingPageViewModel()
    {
        if (System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version is Version version)
        {
            CurrentVersion = version.ToString();
        }
    }

    [RelayCommand]
    async Task LinkToGithubRepo()
    {
        await Launcher.LaunchUriAsync(new Uri("https://github.com/XFEstudio/SpaceEngineersBlueprintEditorInWinUI"));
    }
}
