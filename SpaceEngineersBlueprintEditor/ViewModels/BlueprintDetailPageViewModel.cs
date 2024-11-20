using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using Sandbox.Definitions;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.SpaceEngineersCore;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.Views;
using System.Collections.ObjectModel;
using VRage.Game;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class BlueprintDetailPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private string authorName = "蓝图作者：加载中...";
    [ObservableProperty]
    private string blueprintFileSize = "蓝图大小：加载中...";
    [ObservableProperty]
    private string blueprintPath = "蓝图路径：加载中...";
    [ObservableProperty]
    private string isBlueprintDestructible = "是否可被破坏：加载中...";
    [ObservableProperty]
    private string isBlueprintEditable = "是否可在游戏内编辑：加载中...";
    [ObservableProperty]
    private string totalGridNumber = "网格总数：加载中...";
    [ObservableProperty]
    private string totalCubeNumber = "方块总数：加载中...";
    [ObservableProperty]
    private bool isProgressRingVisible = true;
    private BlueprintInfoViewData? currentBlueprintInfoViewData;
    private MyObjectBuilder_Definitions? currentDefinitions;
    private MyObjectBuilder_ShipBlueprintDefinition? currentBlueprint;
    private readonly INavigationViewService? navigationViewService = GlobalServiceManager.GetService<INavigationViewService>();
    private readonly IMessageService? messageService = GlobalServiceManager.GetService<IMessageService>();
    public ObservableCollection<string> DLCList { get; } = [];
    public ObservableCollection<CubeGridInfo> CubeGridList { get; } = [];
    public ObservableCollection<DefinitionViewData> ComponentList { get; } = [];
    public IBackgroundImageService? BackgroundImageService { get; set; } = GlobalServiceManager.GetService<IBackgroundImageService>();
    public INavigationParameterService<BlueprintInfoViewData> NavigationParameterService { get; set; } = new NavigationParameterService<BlueprintInfoViewData>();
    public BlueprintDetailPageViewModel() => NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;

    private async void NavigationParameterService_ParameterChange(object? sender, BlueprintInfoViewData e)
    {
        if (navigationViewService is not null) navigationViewService.Header = e.Name;
        if (currentBlueprintInfoViewData == e) return;
        AuthorName = "蓝图作者：加载中...";
        BlueprintFileSize = "蓝图大小：加载中...";
        BlueprintPath = "蓝图路径：加载中...";
        IsBlueprintDestructible = "是否可被破坏：加载中...";
        IsBlueprintEditable = "是否可在游戏内编辑：加载中...";
        TotalGridNumber = "网格总数：加载中...";
        TotalCubeNumber = "方块总数：加载中...";
        DLCList.Clear();
        CubeGridList.Clear();
        ComponentList.Clear();
        IsProgressRingVisible = true;
        currentBlueprintInfoViewData = e;
        if (e.NoBlueprint)
        {
            messageService?.ShowMessage("该蓝图不包含蓝图文件（bp.sbc）", "警告", InfoBarSeverity.Warning);
            BlueprintFileSize = $"蓝图大小：{currentBlueprintInfoViewData.FileSize}";
            BlueprintPath = $"蓝图路径：{currentBlueprintInfoViewData.FilePath}";
            IsProgressRingVisible = false;
            return;
        }
        await Helper.Wait(() => SpaceEngineersHelper.IsLoadComplete);
        await LoadBlueprintAsync();
        if (currentBlueprint is not null)
        {
            AuthorName = $"蓝图作者：{currentBlueprint.DisplayName}(Steam ID: {currentBlueprint.OwnerSteamId})";
            BlueprintFileSize = $"蓝图大小：{currentBlueprintInfoViewData.FileSize}";
            BlueprintPath = $"蓝图路径：{currentBlueprintInfoViewData.FilePath}";
            IsBlueprintDestructible = $"是否可被破坏：{(currentBlueprint.CubeGrids.Any(grid => !grid.DestructibleBlocks) ? currentBlueprint.CubeGrids.Any(grid => grid.DestructibleBlocks) ? "部分可被破坏" : "整体不可破坏" : "可被破坏")}";
            IsBlueprintEditable = $"是否可在游戏内编辑：{(currentBlueprint.CubeGrids.Any(grid => !grid.Editable) ? currentBlueprint.CubeGrids.Any(grid => grid.Editable) ? "部分可编辑" : "整体不可编辑" : "可编辑")}";
            CalculateDLC();
            CalculateCubeGrids();
            CalculateComponent();
            IsProgressRingVisible = false;
            messageService?.ShowMessage("蓝图加载完成", "完成", InfoBarSeverity.Success);
        }
        else
        {
            IsProgressRingVisible = false;
            messageService?.ShowMessage("未能加载蓝图", "错误", InfoBarSeverity.Error);
        }
    }

    private async Task LoadBlueprintAsync()
    {
        if (currentBlueprintInfoViewData is not null)
            await Task.Run(() =>
            {
                currentDefinitions = SpaceEngineerDefinitions.Load<MyObjectBuilder_Definitions>(currentBlueprintInfoViewData.FilePath);
                if (currentDefinitions is not null && currentDefinitions.ShipBlueprints is not null && currentDefinitions.ShipBlueprints.Length > 0)
                    currentBlueprint = currentDefinitions.ShipBlueprints[0];
            });
    }

    private void CalculateCubeGrids()
    {
        if (currentBlueprint is not null)
        {
            if (currentBlueprint.CubeGrids is not null)
            {
                int totalCubeCount = 0;
                foreach (var grid in currentBlueprint.CubeGrids)
                    if (grid is not null)
                    {
                        totalCubeCount += grid.CubeBlocks.Count;
                        CubeGridList.Add(new($"网格名称：{grid.DisplayName}", $"方块数量：{grid.CubeBlocks.Count}"));
                    }
                TotalCubeNumber = $"方块总数：{totalCubeCount}";
                TotalGridNumber = $"网格总数：{currentBlueprint.CubeGrids.Length}";
            }
            else if (currentBlueprint.CubeGrid is not null)
            {
                CubeGridList.Add(new($"网格名称：{currentBlueprint.CubeGrid.DisplayName}", $"方块数量：{currentBlueprint.CubeGrid.CubeBlocks.Count}"));
            }
            else
            {
                CubeGridList.Add(new("未找到网格", "方块数量：无"));
            }
        }
    }

    private void CalculateComponent()
    {
        if (currentBlueprint is not null)
        {
            var cubeHashDictionary = new Dictionary<int, int>();
            var componentDictionary = new Dictionary<MyComponentDefinition, int>();
            currentBlueprint.CubeGrids.ForEach(grid => grid.CubeBlocks.ForEach(cube =>
            {
                if (cubeHashDictionary.TryGetValue(cube.SubtypeId.m_hash, out int value))
                    cubeHashDictionary[cube.SubtypeId.m_hash] = ++value;
                else
                    cubeHashDictionary.TryAdd(cube.SubtypeId.m_hash, 1);
            }));
            cubeHashDictionary.ForEach(cubeHashEntry =>
            {
                if (SpaceEngineersHelper.GetDefinition(cubeHashEntry.Key) is MyCubeBlockDefinition definition && definition is not null && definition.Components is not null && definition.Components.Length > 0)
                {
                    definition.Components.ForEach(component =>
                    {
                        if (componentDictionary.ContainsKey(component.Definition))
                            componentDictionary[component.Definition] += component.Count * cubeHashEntry.Value;
                        else
                            componentDictionary.TryAdd(component.Definition, component.Count * cubeHashEntry.Value);
                    });
                }
            });
            componentDictionary.ForEach(componentEntry => ComponentList.Add(new()
            {
                DefinitionBase = componentEntry.Key,
                AdditionalInfo = $"x{componentEntry.Value}",
                ImageSource = componentEntry.Key.Icons is not null && componentEntry.Key.Icons.Length > 0 ? new BitmapImage(new(@$"{AppPath.DefinitionImages}\{FileHelper.ChangeExtension(componentEntry.Key.Icons[0], "png")}")) : null
            }));
        }
    }

    private void CalculateDLC()
    {
        if (currentBlueprint is not null && currentBlueprint.DLCs is not null)
        {
            foreach (var dlc in currentBlueprint.DLCs)
                if (dlc is not null)
                    DLCList.Add(dlc);
        }
        else
        {
            DLCList.Add("无DLC");
        }
    }

    [RelayCommand]
    void GoToEditorPage() => navigationViewService?.NavigateTo(typeof(BlueprintEditPage), new BlueprintModel()
    {
        BlueprintDefinitions = currentDefinitions,
        ViewData = currentBlueprintInfoViewData
    });
}
