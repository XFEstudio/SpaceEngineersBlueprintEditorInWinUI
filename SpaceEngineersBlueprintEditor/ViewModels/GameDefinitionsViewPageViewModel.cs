using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.Utilities;
using System.Collections.ObjectModel;
using System.Diagnostics;
using VRage.Game;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class GameDefinitionsViewPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private string searchText = "";
    [ObservableProperty]
    private bool isProgressRingVisible = true;
    private string currentParameter = "";
    private List<DefinitionViewData> _definitions = [];
    public ObservableCollection<DefinitionViewData> Definitions { get; } = [];
    public INavigationParameterService<string> NavigationParameterService { get; } = new NavigationParameterService<string>();

    public GameDefinitionsViewPageViewModel() => NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;

    private void NavigationParameterService_ParameterChange(object? sender, string e)
    {
        currentParameter = e;
        LoadDefinitions(GetCurrentDefinitions(currentParameter));
        IsProgressRingVisible = false;
    }

    private void LoadDefinitions(IEnumerable<MyDefinitionBase> myDefinitions)
    {
        foreach (var definition in myDefinitions)
            if (definition is not null)
            {
                SpaceEngineersHelper.DefinitionImageList.TryGetValue(definition, out var bitmapImage);
                var definitionViewData = new DefinitionViewData(bitmapImage, definition);
                _definitions.Add(definitionViewData);
                Definitions.Add(definitionViewData);
            }
    }

    //private async Task LoadDefinitions(IEnumerable<MyDefinitionBase> myDefinitions)
    //{
    //    foreach (var definition in myDefinitions)
    //        if (definition is not null)
    //        {
    //            var definitionViewData = new DefinitionViewData(await FileHelper.ToBitmap($@"{SpaceEngineersPath.SpaceEngineerContentPath}\{definition.Icons[0]}"), definition);
    //            _definitions.Add(definitionViewData);
    //            Definitions.Add(definitionViewData);
    //        }
    //}

    private IEnumerable<DefinitionViewData> SearchDefinitions(string definitionsName) => _definitions.Where(definition => definition.DefinitionBase.DisplayNameText is not null && definition.DefinitionBase.DisplayNameText.Contains(definitionsName));

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
