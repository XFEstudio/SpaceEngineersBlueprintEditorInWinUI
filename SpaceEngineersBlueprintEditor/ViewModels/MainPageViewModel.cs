using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.Views;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class MainPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool isProgressRingVisible;
    private readonly IAutoNavigationService? navigationService = GlobalServiceManager.GetService<IAutoNavigationService>();
    public IFileDropService FileDropService { get; set; } = new BlueprintDropService();

    public MainPageViewModel()
    {
        FileDropService.Drop += FileDropService_Drop;
    }

    private void FileDropService_Drop(object? sender, (string, DragEventArgs) e)
    {
        if (File.Exists(e.Item1))
            navigationService?.NavigateTo<BlueprintDetailPage>(SpaceEngineersHelper.LoadBlueprintInfo(e.Item1)?.ToBlueprintInfoViewData());
    }

    [RelayCommand]
    void ViewBlueprintsList() => navigationService?.NavigateTo<BlueprintsViewPage>("Local");

    [RelayCommand]
    async Task OpenBlueprintInFolder()
    {
        var openPicker = new FileOpenPicker();
        InitializeWithWindow.Initialize(openPicker, WindowNative.GetWindowHandle(App.MainWindow));
        openPicker.ViewMode = PickerViewMode.List;
        openPicker.FileTypeFilter.Add(".sbc");
        if (await openPicker.PickSingleFileAsync() is StorageFile file)
            navigationService?.NavigateTo<BlueprintDetailPage>(SpaceEngineersHelper.LoadBlueprintInfo(file.Path)?.ToBlueprintInfoViewData());
    }
}
