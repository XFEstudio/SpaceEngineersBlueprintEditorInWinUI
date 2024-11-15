using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.Utilities;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class BlueprintDetailPageViewModel : ViewModelBase
{
    private INavigationViewService? navigationViewService = GlobalServiceManager.GetService<INavigationViewService>();
    public INavigationParameterService<BlueprintInfoViewData> NavigationParameterService { get; set; } = new NavigationParameterService<BlueprintInfoViewData>();
    public BlueprintDetailPageViewModel()
    {
        NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;
    }

    private void NavigationParameterService_ParameterChange(object? sender, BlueprintInfoViewData e)
    {
        if (navigationViewService is not null) navigationViewService.Header = e.Name;
    }
}
