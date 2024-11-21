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
    private string? selectedEditEnum;
    [ObservableProperty]
    private TreeViewNode? selectedTreeViewNode;
    private BlueprintModel? currentParameter;
    private MyObjectBuilder_Definitions? currentDefinitions;
    private MyObjectBuilder_ShipBlueprintDefinition? currentShipBlueprint;
    private readonly IMessageService? messageService = GlobalServiceManager.GetService<IMessageService>();
    private readonly INavigationViewService? navigationViewService = GlobalServiceManager.GetService<INavigationViewService>();
    public ObservableCollection<string> EditValueEnumList { get; set; } = [];
    public IBackgroundImageService? BackgroundImageService { get; set; } = GlobalServiceManager.GetService<IBackgroundImageService>();
    public INavigationParameterService<BlueprintModel> NavigationParameterService { get; set; } = new NavigationParameterService<BlueprintModel>();
    public IDialogService DialogService { get; set; } = new DialogService();
    public BlueprintEditPageViewModel() => NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;

    partial void OnSelectedTreeViewNodeChanged(TreeViewNode? value)
    {

    }

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
                var parent = new BlueprintPropertyViewData
                {
                    Value = currentShipBlueprint,
                    Name = "飞船蓝图",
                    Type = currentShipBlueprint.GetType()
                };
                SpaceEngineersHelper.AnalyzeBlueprint(parent);
                foreach (var child in parent.Children)
                {
                    BlueprintEditPage.Current?.propertyTreeView.RootNodes.Add(new TreeViewNode
                    {
                        Content = child,
                        HasUnrealizedChildren = !child.IsBasicType
                    });
                }
                foreach (var grid in currentShipBlueprint.CubeGrids)
                {
                    var type = grid.GetType();
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
        messageService?.ShowMessage("转换完成", "完成", InfoBarSeverity.Success);
    }
}
