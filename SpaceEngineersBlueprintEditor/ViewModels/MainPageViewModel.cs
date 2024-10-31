using CommunityToolkit.Mvvm.Input;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.Views;
using System.Diagnostics;
using Windows.Storage.Pickers;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class MainPageViewModel : ViewModelBase
{
    private INavigationService? navigationService = GlobalServiceManager.GetService<INavigationService>();
    public IFileDropService FileDropService { get; set; } = new BlueprintDropService();

    public MainPageViewModel()
    {
        FileDropService.Drop += FileDropService_Drop;
    }

    private void FileDropService_Drop(object? sender, (string, DragEventArgs) e)
    {

    }

    [RelayCommand]
    void ViewBlueprintsList() => navigationService?.NavigateTo<BlueprintsViewPage>("Local");

    [RelayCommand]
    async Task OpenBlueprintInFolder()
    {
        var openPicker = new FileOpenPicker();
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow));
        openPicker.ViewMode = PickerViewMode.List;
        openPicker.FileTypeFilter.Add(".sbc");
        var file = await openPicker.PickSingleFileAsync();
        if (file is not null)
            Debug.WriteLine(file.Path);
    }
}
