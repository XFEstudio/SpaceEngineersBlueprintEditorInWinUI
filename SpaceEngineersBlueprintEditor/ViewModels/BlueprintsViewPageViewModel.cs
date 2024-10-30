using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.Utilities;
using System.Collections.ObjectModel;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class BlueprintsViewPageViewModel : ViewModelBase
{
    public INavigationParameterService NavigationParameterService { get; set; } = new NavigationParameterService();
    public ObservableCollection<BlueprintInfoViewData> BlueprintInfoViewDataList { get; set; } = [];

    public BlueprintsViewPageViewModel()
    {
        NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;
    }

    private void LoadBlueprints(List<BlueprintInfo> blueprintInfoList)
    {
        foreach (var info in blueprintInfoList)
            BlueprintInfoViewDataList.Add(info.ToBlueprintInfoViewData());
    }

    private void NavigationParameterService_ParameterChange(object? sender, object? e)
    {
        if (e is string parameter)
            switch (parameter)
            {
                case "Local":
                    BlueprintInfoViewDataList.Clear();
                    LoadBlueprints(BlueprintsManager.LocalBlueprints);
                    break;
                case "Cloud":
                    BlueprintInfoViewDataList.Clear();
                    LoadBlueprints(BlueprintsManager.CloudBlueprints);
                    break;
                case "Workshop":
                    BlueprintInfoViewDataList.Clear();
                    LoadBlueprints(BlueprintsManager.WorkshopBlueprints);
                    break;
                default:
                    break;
            }
    }
}
