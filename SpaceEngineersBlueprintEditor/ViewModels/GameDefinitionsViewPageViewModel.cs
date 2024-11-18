using CommunityToolkit.Mvvm.ComponentModel;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.Utilities.Helpers;
using System.Collections.ObjectModel;
using VRage.Game;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class GameDefinitionsViewPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private string searchText = "";
    private string currentParameter = "";
    public ObservableCollection<DefinitionViewData> Definitions { get; } = [];
    public INavigationParameterService<string> NavigationParameterService { get; } = new NavigationParameterService<string>();

    public GameDefinitionsViewPageViewModel() => NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;

    private void NavigationParameterService_ParameterChange(object? sender, string e)
    {
        currentParameter = e;
        LoadDefinitions(GetCurrentDefinitions(currentParameter));
    }

    private void LoadDefinitions(IEnumerable<MyDefinitionBase> myDefinitions)
    {
        foreach (var definition in myDefinitions)
            if (definition is not null)
                Definitions.Add(definition.TpDefinitionViewData());
    }

    private static IEnumerable<MyDefinitionBase> SearchDefinitions(string searchMode, string definitionsName) => GetCurrentDefinitions(searchMode).Where(definition => definition.DisplayNameString.Contains(definitionsName));

    private static IEnumerable<MyDefinitionBase> GetCurrentDefinitions(string currentLocation) => currentLocation switch
    {
        "Cubes" => SpaceEngineersHelper.CubeBlockDefinitions,
        "Components" => SpaceEngineersHelper.ComponentDefinitions,
        "Items" => SpaceEngineersHelper.ItemDefinitions,
        _ => throw new NotImplementedException()
    };

    partial void OnSearchTextChanged(string value)
    {
        Definitions.Clear();
        LoadDefinitions(SearchDefinitions(currentParameter, value));
    }
}
