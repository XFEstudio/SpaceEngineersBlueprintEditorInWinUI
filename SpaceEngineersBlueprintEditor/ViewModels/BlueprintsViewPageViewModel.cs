using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public class BlueprintsViewPageViewModel : ViewModelBase
{
    public INavigationParameterService NavigationParameterService { get; set; } = new NavigationParameterService();

    public BlueprintsViewPageViewModel()
    {
        NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;
    }

    private void NavigationParameterService_ParameterChange(object? sender, object? e)
    {

    }
}
