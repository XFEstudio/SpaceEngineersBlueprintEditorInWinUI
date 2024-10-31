using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.Utilities;
using System.Collections.ObjectModel;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class BlueprintsViewPageViewModel : ViewModelBase
{
    private string currentParameter = "";
    private readonly IMessageService? messageService = GlobalServiceManager.GetService<IMessageService>();
    [ObservableProperty]
    private string searchText = "";
    public INavigationParameterService<string> NavigationParameterService { get; set; } = new NavigationParameterService<string>();
    public ObservableCollection<BlueprintInfoViewData> BlueprintInfoViewDataList { get; set; } = [];

    public BlueprintsViewPageViewModel()
    {
        NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;
    }

    partial void OnSearchTextChanged(string value)
    {
        BlueprintInfoViewDataList.Clear();
        LoadBlueprints(SearchBlueprints(currentParameter, value).ToList());
    }

    private static IEnumerable<BlueprintInfo> SearchBlueprints(string searchMode, string blueprintName) => GetCurrentBlueprints(searchMode).Where(blueprint => blueprint.Name.Contains(blueprintName));

    private static List<BlueprintInfo> GetCurrentBlueprints(string currentLocation) => currentLocation switch
    {
        "Local" => BlueprintsManager.LocalBlueprints,
        "Cloud" => BlueprintsManager.CloudBlueprints,
        "Workshop" => BlueprintsManager.WorkshopBlueprints,
        _ => throw new NotImplementedException()
    };

    private void LoadCurrentBlueprints() => LoadBlueprints(GetCurrentBlueprints(currentParameter));

    private void LoadBlueprints(List<BlueprintInfo> blueprintInfoList)
    {
        foreach (var info in blueprintInfoList)
            BlueprintInfoViewDataList.Add(info.ToBlueprintInfoViewData());
    }

    private async void NavigationParameterService_ParameterChange(object? sender, string e)
    {
        currentParameter = e;
        await BlueprintsManager.LoadBlueprintsAsync();
        BlueprintInfoViewDataList.Clear();
        LoadCurrentBlueprints();
    }


    [RelayCommand]
    async Task RefreshBlueprints()
    {
        await BlueprintsManager.LoadBlueprintsAsync();
        if (SearchText == string.Empty)
            LoadCurrentBlueprints();
        else
            OnSearchTextChanged(SearchText);
        messageService?.ShowMessage("刷新成功", "完成", InfoBarSeverity.Success);
    }
}
