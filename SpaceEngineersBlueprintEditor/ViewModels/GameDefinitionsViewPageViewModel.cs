using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using Sandbox.Definitions;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.SpaceEngineersCore;
using SpaceEngineersBlueprintEditor.Utilities;
using System.Collections.ObjectModel;
using VRage.Game;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class GameDefinitionsViewPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private string searchText = "";
    [ObservableProperty]
    private bool isProgressRingVisible = true;
    [ObservableProperty]
    private DefinitionViewData? selectedDefinitionViewData;
    private string currentParameter = "";
    private readonly List<DefinitionViewData> _definitions = [];
    public ObservableCollection<DefinitionViewData> Definitions { get; } = [];
    public INavigationParameterService<string> NavigationParameterService { get; } = new NavigationParameterService<string>();

    public GameDefinitionsViewPageViewModel() => NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;

    private async void NavigationParameterService_ParameterChange(object? sender, string e)
    {
        currentParameter = e;
        while (!Initializer.IsDefinitionsLoadComplete) { await Task.Delay(100); }
        LoadDefinitions(GetCurrentDefinitions(currentParameter));
        IsProgressRingVisible = false;
    }

    private void LoadDefinitions(IEnumerable<MyDefinitionBase> myDefinitions)
    {
        foreach (var definition in myDefinitions)
            if (definition is not null)
            {
                var definitionViewData = new DefinitionViewData
                {
                    DefinitionBase = definition,
                    ImageSource = definition.Icons is not null && definition.Icons.Length > 0 ? new BitmapImage(new(@$"{AppPath.DefinitionImages}\{FileHelper.ChangeExtension(definition.Icons[0], "png")}")) : null,
                    CubeSize = definition is MyCubeBlockDefinition myCubeBlockDefinition ? myCubeBlockDefinition.CubeSize == MyCubeSize.Small ? "\uE744" : "\uE978" : null
                };
                _definitions.Add(definitionViewData);
                Definitions.Add(definitionViewData);
            }
    }

    private IEnumerable<DefinitionViewData> SearchDefinitions(string definitionsName) => _definitions.Where(definition => definition.DefinitionBase.DisplayNameText is not null && definition.DefinitionBase.DisplayNameText.Contains(definitionsName, StringComparison.OrdinalIgnoreCase));

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
        foreach (var definition in SearchDefinitions(value))
            Definitions.Add(definition);
    }
}
