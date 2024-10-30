using CommunityToolkit.Mvvm.Input;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.Views;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class BlueprintEditPageViewModel : ViewModelBase
{
    private INavigationService? navigationService = GlobalServiceManager.GetService<INavigationService>();
    public IFileDropService FileDropService { get; set; } = new BlueprintDropService();
    public INavigationParameterService NavigationParameterService { get; set; } = new NavigationParameterService();

    public BlueprintEditPageViewModel()
    {
        NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;
        FileDropService.Drop += FileDropService_Drop;
    }

    private void NavigationParameterService_ParameterChange(object? sender, object? e)
    {

    }

    private void FileDropService_Drop(object? sender, (string, DragEventArgs) e)
    {

    }

    [RelayCommand]
    void ViewBlueprintsList() => navigationService?.NavigateTo<BlueprintsViewPage>("Local");
}
