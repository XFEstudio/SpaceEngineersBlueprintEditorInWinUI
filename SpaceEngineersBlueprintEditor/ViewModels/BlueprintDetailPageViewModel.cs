using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class BlueprintDetailPageViewModel : ViewModelBase
{
    public INavigationParameterService<BlueprintInfoViewData> NavigationParameterService { get; set; } = new NavigationParameterService<BlueprintInfoViewData>();
    public BlueprintDetailPageViewModel()
    {
        NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;
    }

    private void NavigationParameterService_ParameterChange(object? sender, BlueprintInfoViewData e)
    {

    }
}
