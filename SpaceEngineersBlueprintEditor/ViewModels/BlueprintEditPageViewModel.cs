using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.Views;
using System.Collections.ObjectModel;
using System.Reflection;
using VRage.Game;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class BlueprintEditPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool isProgressRingVisible = true;
    [ObservableProperty]
    private string? editValueText;
    [ObservableProperty]
    private TreeViewNode? selectedTreeViewNode;
    private BlueprintModel? currentParameter;
    private MyObjectBuilder_Definitions? currentDefinitions;
    private MyObjectBuilder_ShipBlueprintDefinition? currentShipBlueprint;
    private readonly IMessageService? messageService = GlobalServiceManager.GetService<IMessageService>();
    private readonly INavigationViewService? navigationViewService = GlobalServiceManager.GetService<INavigationViewService>();
    public ObservableCollection<TreeViewNode> BlueprintPropertyNodeList { get; set; } = [];
    public ObservableCollection<BlueprintPropertyViewData> ShipGridPropertyList { get; set; } = [];
    public IBackgroundImageService? BackgroundImageService { get; set; } = GlobalServiceManager.GetService<IBackgroundImageService>();
    public INavigationParameterService<BlueprintModel> NavigationParameterService { get; set; } = new NavigationParameterService<BlueprintModel>();
    public IDialogService DialogService { get; set; } = new DialogService();
    public BlueprintEditPageViewModel() => NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;

    private void NavigationParameterService_ParameterChange(object? sender, BlueprintModel e)
    {
        if (e.BlueprintDefinitions is not null)
        {
            if (navigationViewService is not null) navigationViewService.Header = null;
            currentParameter = e;
            currentDefinitions = e.BlueprintDefinitions;
            if (currentDefinitions.ShipBlueprints is not null && currentDefinitions.ShipBlueprints.Length > 0)
            {
                currentShipBlueprint = currentDefinitions.ShipBlueprints[0];
                var memberInfoList = currentShipBlueprint.GetType().GetMembers();
                var parent = new BlueprintPropertyViewData
                {
                    Value = currentShipBlueprint,
                    Name = "飞船蓝图",
                    Type = currentShipBlueprint.GetType()
                };
                foreach (var memberInfo in memberInfoList)
                {
                    if (memberInfo is FieldInfo fieldInfo && fieldInfo.IsPublic)
                    {
                        var blueprintPropertyViewData = new BlueprintPropertyViewData()
                        {
                            Type = fieldInfo.FieldType,
                            Name = memberInfo.Name,
                            Parent = parent,
                            Value = fieldInfo.GetValue(currentShipBlueprint)
                        };
                        var node = new TreeViewNode
                        {
                            Content = blueprintPropertyViewData,
                            HasUnrealizedChildren = blueprintPropertyViewData.IsNotBasicType
                        };
                        BlueprintEditPage.Current?.propertyTreeView.RootNodes.Add(node);
                    }
                    else if (memberInfo is PropertyInfo propertyInfo && propertyInfo.CanWrite && propertyInfo.IsMemberPublic())
                    {
                        var blueprintPropertyViewData = new BlueprintPropertyViewData()
                        {
                            Type = propertyInfo.PropertyType,
                            Name = memberInfo.Name,
                            Parent = parent,
                            Value = propertyInfo.GetValue(currentShipBlueprint)
                        };
                        var node = new TreeViewNode
                        {
                            Content = blueprintPropertyViewData,
                            HasUnrealizedChildren = blueprintPropertyViewData.IsNotBasicType
                        };
                        BlueprintEditPage.Current?.propertyTreeView.RootNodes.Add(node);
                    }
                }
                foreach (var grid in currentShipBlueprint.CubeGrids)
                {
                    var type = grid.GetType();
                    ShipGridPropertyList.Add(new()
                    {
                        Type = type,
                        Name = grid.DisplayName,
                        Value = grid
                    });
                }
            }
            else
            {
                messageService?.ShowMessage("未能加载飞船蓝图定义", "错误", InfoBarSeverity.Error);
            }
        }
        else
        {
            messageService?.ShowMessage("未能找到蓝图定义", "错误", InfoBarSeverity.Error);
        }
        IsProgressRingVisible = false;
    }

    [RelayCommand]
    void OpenBlueprint()
    {

    }

    [RelayCommand]
    void Save()
    {

    }

    [RelayCommand]
    void ConvertBlueprint(string command)
    {
        if (currentShipBlueprint is not null)
            switch (command)
            {
                case "Destructible":
                    foreach (var grid in currentShipBlueprint.CubeGrids)
                        grid.DestructibleBlocks = true;
                    break;
                case "Indestructible":
                    foreach (var grid in currentShipBlueprint.CubeGrids)
                        grid.DestructibleBlocks = false;
                    break;
                case "Editable":
                    foreach (var grid in currentShipBlueprint.CubeGrids)
                        grid.Editable = true;
                    break;
                case "Non-Editable":
                    foreach (var grid in currentShipBlueprint.CubeGrids)
                        grid.Editable = false;
                    break;
                default:
                    break;
            }
    }

    [RelayCommand]
    void Add()
    {

    }

    [RelayCommand]
    async Task Edit()
    {
        if (SelectedTreeViewNode is not null && SelectedTreeViewNode.Content is BlueprintPropertyViewData blueprintPropertyViewData)
        {
            EditValueText = blueprintPropertyViewData.ValueInString;
            if (blueprintPropertyViewData.Parent is not null && blueprintPropertyViewData.Name is not null && blueprintPropertyViewData.Parent.Type is not null && await DialogService.ShowDialog("editValueContentDialog") == ContentDialogResult.Primary)
            {
                blueprintPropertyViewData.Parent.Type.GetMember(blueprintPropertyViewData.Name)
            }
        }
    }

    [RelayCommand]
    void Delete()
    {

    }
}
