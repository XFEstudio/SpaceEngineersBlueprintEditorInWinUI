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
using XFEExtension.NetCore.StringExtension;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class GameDefinitionsViewPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private string searchText = "";
    [ObservableProperty]
    private string propertiesSearchText = "";
    [ObservableProperty]
    private bool isProgressRingVisible = true;
    [ObservableProperty]
    private DefinitionViewData? selectedDefinitionViewData;
    private string currentParameter = "";
    private readonly List<DefinitionViewData> _definitions = [];
    private readonly List<PropertyViewData> _propertiesInfo = [];
    public ObservableCollection<DefinitionViewData> Definitions { get; } = [];
    public ObservableCollection<PropertyViewData> PropertiesInfo { get; } = [];
    public INavigationParameterService<string> NavigationParameterService { get; } = new NavigationParameterService<string>();
    public IDefinitionPropertiesDisplayService<DefinitionViewData> DefinitionPropertiesDisplayService { get; } = new DefinitionPropertiesDisplayService<DefinitionViewData>();

    public GameDefinitionsViewPageViewModel()
    {
        NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;
        DefinitionPropertiesDisplayService.SelectionChanged += DefinitionPropertiesDisplayService_SelectionChanged;
    }

    private void DefinitionPropertiesDisplayService_SelectionChanged(ItemsView sender, ItemsViewSelectionChangedEventArgs args)
    {
        if (sender.SelectedItem is DefinitionViewData definitionViewData)
        {
            SelectedDefinitionViewData = definitionViewData;
            PropertiesInfo.Clear();
            foreach (var definition in SelectedDefinitionViewData.DefinitionBase.GetType().GetFields())
            {
                if (definition.GetValue(SelectedDefinitionViewData.DefinitionBase) is object value)
                {
                    var valueString = value.ToString();
                    var propertyViewData = new PropertyViewData
                    {
                        PropertyName = definition.Name,
                        Value = string.IsNullOrWhiteSpace(valueString) ? "值为空" : valueString,
                        IsValueType = value.GetType().IsValueType
                    };
                    _propertiesInfo.Add(propertyViewData);
                    PropertiesInfo.Add(propertyViewData);
                }
            }
        }
    }

    private async void NavigationParameterService_ParameterChange(object? sender, string e)
    {
        currentParameter = e;
        while (!Initializer.IsDefinitionsLoadComplete) { await Task.Delay(100); }
        LoadDefinitions(GetCurrentDefinitions(currentParameter));
        IsProgressRingVisible = false;
        while (!DefinitionPropertiesDisplayService.IsPageLoaded) { await Task.Delay(100); }
        DefinitionPropertiesDisplayService.Select(Definitions.First());
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

    private IEnumerable<PropertyViewData> SearchProperties(string propertiesName) => _propertiesInfo.Where(property => property.PropertyName.Contains(propertiesName, StringComparison.OrdinalIgnoreCase) || (property.Value is not null && property.Value.ToString() is string valueString && valueString.Contains(propertiesName, StringComparison.OrdinalIgnoreCase)));

    private static IEnumerable<MyDefinitionBase> GetCurrentDefinitions(string currentLocation) => currentLocation switch
    {
        "Cubes" => SpaceEngineersHelper.CubeBlockDefinitions.OrderBy(definition => definition.CubeSize),
        "Components" => SpaceEngineersHelper.ComponentDefinitions,
        "Items" => SpaceEngineersHelper.ItemDefinitions,
        "Scenarios" => SpaceEngineersHelper.ScenarioDefinition,
        _ => throw new NotImplementedException()
    };

    partial void OnSearchTextChanged(string value)
    {
        Definitions.Clear();
        foreach (var definition in SearchDefinitions(value))
            Definitions.Add(definition);
    }

    partial void OnPropertiesSearchTextChanged(string value)
    {
        PropertiesInfo.Clear();
        foreach (var property in SearchProperties(value))
            PropertiesInfo.Add(property);
    }
}
