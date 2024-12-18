﻿using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using Sandbox.Definitions;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.SpaceEngineersCore;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.Views;
using System.Collections.ObjectModel;
using System.Linq;
using VRage.Game;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class GameDefinitionsViewPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private string searchText = "";
    [ObservableProperty]
    private string propertiesSearchText = "";
    [ObservableProperty]
    private bool isUIVisible = false;
    [ObservableProperty]
    private DefinitionViewData? selectedDefinitionViewData;
    private string currentParameter = "";
    private readonly List<DefinitionViewData> _definitions = [];
    private readonly List<PropertyViewData> _propertiesInfo = [];
    private readonly ILoadingService? loadingService = GlobalServiceManager.GetService<ILoadingService>();
    public ObservableCollection<DefinitionViewData> Definitions { get; } = [];
    public ObservableCollection<PropertyViewData> PropertiesInfo { get; } = [];
    public INavigationParameterService<string> NavigationParameterService { get; } = new NavigationParameterService<string>();
    public IItemsViewDisplayService<DefinitionViewData> ItemsViewDisplayService { get; } = new ItemsViewDisplayService<DefinitionViewData>();

    public GameDefinitionsViewPageViewModel()
    {
        NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;
        ItemsViewDisplayService.SelectionChanged += DefinitionPropertiesDisplayService_SelectionChanged;
    }

    private void DefinitionPropertiesDisplayService_SelectionChanged(ItemsView sender, ItemsViewSelectionChangedEventArgs args)
    {
        if (sender.SelectedItem is DefinitionViewData definitionViewData)
        {
            SelectedDefinitionViewData = definitionViewData;
            PropertiesInfo.Clear();
            _propertiesInfo.Clear();
            foreach (var definition in SelectedDefinitionViewData.DefinitionBase.GetType().GetFields())
            {
                if (definition.GetValue(SelectedDefinitionViewData.DefinitionBase) is object value)
                {
                    var valueString = value.ToString();
                    var isValueType = value.GetType().IsValueType || value.GetType().IsEnum || value.GetType() == typeof(string);
                    var propertyViewData = new PropertyViewData
                    {
                        PropertyName = definition.Name,
                        Value = string.IsNullOrWhiteSpace(valueString) ? isValueType ? "EmptyValue".GetLocalized() : "NullObject".GetLocalized() : valueString,
                        IsValueType = isValueType
                    };
                    _propertiesInfo.Add(propertyViewData);
                }
            }
            OnPropertiesSearchTextChanged(PropertiesSearchText);
        }
    }

    private async void NavigationParameterService_ParameterChange(object? sender, string? e)
    {
        if (e is null || NavigationParameterService.SameAsLast)
            return;
        loadingService?.StartLoading<GameDefinitionsViewPage>($"{"Loading".GetLocalized()} {e.ToLower()} {"Definitions".GetLocalized().ToLower()}...");
        currentParameter = e;
        await Helper.Wait(() => Initializer.IsDefinitionsLoadComplete && ItemsViewDisplayService.IsPageLoaded);
        LoadDefinitions(GetCurrentDefinitions(currentParameter));
        IsUIVisible = true;
        loadingService?.StopLoading<GameDefinitionsViewPage>();
        ItemsViewDisplayService.Select(Definitions.First());
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

    private IEnumerable<DefinitionViewData> SearchDefinitions(string definitionsName) => _definitions.Where(definition => definition.DefinitionBase.DisplayNameText is not null && definition.DefinitionBase.DisplayNameText.Contains(definitionsName, StringComparison.OrdinalIgnoreCase)).OrderBy(definition => definition.DefinitionBase.DisplayNameText);

    private IEnumerable<PropertyViewData> SearchProperties(string propertiesName) => _propertiesInfo.Where(property => property.PropertyName.Contains(propertiesName, StringComparison.OrdinalIgnoreCase) || (property.Value is not null && property.Value.ToString() is string valueString && valueString.Contains(propertiesName, StringComparison.OrdinalIgnoreCase))).OrderBy(property => property.PropertyName);

    private static IEnumerable<MyDefinitionBase> GetCurrentDefinitions(string currentLocation) => currentLocation switch
    {
        "Cubes" => SpaceEngineersHelper.CubeBlockDefinitions.OrderBy(definition => definition.DisplayNameText),
        "Components" => SpaceEngineersHelper.ComponentDefinitions.OrderBy(definition => definition.DisplayNameText),
        "Items" => SpaceEngineersHelper.ItemDefinitions.OrderBy(definition => definition.DisplayNameText),
        "Scenarios" => SpaceEngineersHelper.ScenarioDefinition.OrderBy(definition => definition.DisplayNameText),
        _ => throw new NotImplementedException()
    };

    partial void OnSearchTextChanged(string value)
    {
        Definitions.Clear();
        SearchDefinitions(value).ForEach(Definitions.Add);
    }

    partial void OnPropertiesSearchTextChanged(string value)
    {
        PropertiesInfo.Clear();
        SearchProperties(value).ForEach(PropertiesInfo.Add);
    }
}
