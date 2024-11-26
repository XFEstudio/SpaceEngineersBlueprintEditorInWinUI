using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using Sandbox.Definitions;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.Views;
using System.Collections.ObjectModel;
using VRage.Game;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class BlueprintDetailPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private string authorName = $"{"BlueprintDetailPage_BlueprintAuthor".GetLocalized()}: {"Loading".GetLocalized()}...";
    [ObservableProperty]
    private string blueprintFileSize = $"{"BlueprintDetailPage_BlueprintSize".GetLocalized()}: {"Loading".GetLocalized()}...";
    [ObservableProperty]
    private string blueprintPath = $"{"BlueprintDetailPage_BlueprintPath".GetLocalized()}: {"Loading".GetLocalized()}...";
    [ObservableProperty]
    private string isBlueprintDestructible = $"{"BlueprintDetailPage_BlueprintDestructible".GetLocalized()}: {"Loading".GetLocalized()}...";
    [ObservableProperty]
    private string isBlueprintEditable = $"{"BlueprintDetailPage_BlueprintEditable".GetLocalized()}: {"Loading".GetLocalized()}...";
    [ObservableProperty]
    private string totalGridNumber = $"{"BlueprintDetailPage_BlueprintTotalGrids".GetLocalized()}: {"Loading".GetLocalized()}...";
    [ObservableProperty]
    private string totalCubeNumber = $"{"BlueprintDetailPage_BlueprintTotalCubes".GetLocalized()}: {"Loading".GetLocalized()}...";
    private BlueprintInfoViewData? currentBlueprintInfoViewData;
    private MyObjectBuilder_Definitions? currentDefinitions;
    private MyObjectBuilder_ShipBlueprintDefinition? currentBlueprint;
    private readonly ILoadingService? loadingService = GlobalServiceManager.GetService<ILoadingService>();
    private readonly INavigationViewService? navigationViewService = GlobalServiceManager.GetService<INavigationViewService>();
    private readonly IMessageService? messageService = GlobalServiceManager.GetService<IMessageService>();
    public bool IsLoadingInProgress { get; set; }
    public ObservableCollection<string> DLCList { get; } = [];
    public ObservableCollection<CubeGridInfo> CubeGridList { get; } = [];
    public ObservableCollection<DefinitionViewData> ComponentList { get; } = [];
    public IBackgroundImageService? BackgroundImageService { get; set; } = GlobalServiceManager.GetService<IBackgroundImageService>();
    public INavigationParameterService<BlueprintInfoViewData> NavigationParameterService { get; set; } = new NavigationParameterService<BlueprintInfoViewData>();
    public BlueprintDetailPageViewModel() => NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;

    private async void NavigationParameterService_ParameterChange(object? sender, BlueprintInfoViewData? e)
    {
        if (e is null) return;
        if (navigationViewService is not null) navigationViewService.Header = e.Name;
        BackgroundImageService?.SetBackgroundImage(e.BlueprintImage);
        AuthorName = $"{"BlueprintDetailPage_BlueprintAuthor".GetLocalized()}: {"Loading".GetLocalized()}...";
        BlueprintFileSize = $"{"BlueprintDetailPage_BlueprintSize".GetLocalized()}: {"Loading".GetLocalized()}...";
        BlueprintPath = $"{"BlueprintDetailPage_BlueprintPath".GetLocalized()}: {"Loading".GetLocalized()}...";
        IsBlueprintDestructible = $"{"BlueprintDetailPage_BlueprintDestructible".GetLocalized()}: {"Loading".GetLocalized()}...";
        IsBlueprintEditable = $"{"BlueprintDetailPage_BlueprintEditable".GetLocalized()}: {"Loading".GetLocalized()}...";
        TotalGridNumber = $"{"BlueprintDetailPage_BlueprintTotalGrids".GetLocalized()}: {"Loading".GetLocalized()}...";
        TotalCubeNumber = $"{"BlueprintDetailPage_BlueprintTotalCubes".GetLocalized()}: {"Loading".GetLocalized()}...";
        DLCList.Clear();
        CubeGridList.Clear();
        ComponentList.Clear();
        if (!NavigationParameterService.SameAsLast)
        {
            IsLoadingInProgress = true;
            loadingService?.StartLoading<BlueprintDetailPage>($"{"LoadingBlueprint".GetLocalized()}...");
            currentBlueprintInfoViewData = e;
            await Helper.Wait(() => SpaceEngineersHelper.IsLoadComplete);
            currentDefinitions = await SpaceEngineersHelper.LoadBlueprintAsync(currentBlueprintInfoViewData.FilePath);
            if (currentDefinitions is not null && currentDefinitions.ShipBlueprints is not null && currentDefinitions.ShipBlueprints.Length > 0)
                currentBlueprint = currentDefinitions.ShipBlueprints[0];
            messageService?.ShowMessage("BlueprintLoadComplete".GetLocalized(), "Complete".GetLocalized(), InfoBarSeverity.Success);
            loadingService?.StopLoading<BlueprintDetailPage>();
            IsLoadingInProgress = false;
            IsLoadingInProgress = false;
        }
        await Helper.Wait(() => !IsLoadingInProgress);
        loadingService?.StartLoading<BlueprintDetailPage>($"{"CalculatingBlueprint".GetLocalized()}...");
        if (e.NoBlueprint)
        {
            messageService?.ShowMessage("Warning_BlueprintContainsNoFile".GetLocalized(), "Warning".GetLocalized(), InfoBarSeverity.Warning);
            BlueprintFileSize = $"{"BlueprintDetailPage_BlueprintSize".GetLocalized()}: {currentBlueprintInfoViewData!.FileSize}";
            BlueprintPath = $"{"BlueprintDetailPage_BlueprintPath".GetLocalized()}: {currentBlueprintInfoViewData!.FilePath}";
            loadingService?.StopLoading<BlueprintDetailPage>();
            return;
        }
        if (currentBlueprint is not null)
        {
            AuthorName = $"{"BlueprintDetailPage_BlueprintAuthor".GetLocalized()}: {currentBlueprint.DisplayName}(Steam ID: {currentBlueprint.OwnerSteamId})";
            BlueprintFileSize = $"{"BlueprintDetailPage_BlueprintSize".GetLocalized()}: {currentBlueprintInfoViewData!.FileSize}";
            BlueprintPath = $"{"BlueprintDetailPage_BlueprintPath".GetLocalized()}: {currentBlueprintInfoViewData!.FilePath}";
            IsBlueprintDestructible = $"{"BlueprintDetailPage_BlueprintDestructible".GetLocalized()}: {(currentBlueprint.CubeGrids.Any(grid => !grid.DestructibleBlocks) ? currentBlueprint.CubeGrids.Any(grid => grid.DestructibleBlocks) ? "BlueprintDetailPage_PartiallyDestructible".GetLocalized() : "BlueprintDetailPage_OverallIndestructible".GetLocalized() : "BlueprintDetailPage_Destructible".GetLocalized())}";
            IsBlueprintEditable = $"{"BlueprintDetailPage_BlueprintEditable".GetLocalized()}: {(currentBlueprint.CubeGrids.Any(grid => !grid.Editable) ? currentBlueprint.CubeGrids.Any(grid => grid.Editable) ? "BlueprintDetailPage_PartiallyEditable".GetLocalized() : "BlueprintDetailPage_OverallNonEditable".GetLocalized() : "BlueprintDetailPage_Editable".GetLocalized())}";
            CalculateDLC();
            CalculateCubeGrids();
            CalculateComponent();
        }
        else
        {
            messageService?.ShowMessage("Error_CantLoadingBlueprint".GetLocalized(), "Error".GetLocalized(), InfoBarSeverity.Error);
        }
        loadingService?.StopLoading<BlueprintDetailPage>();
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
                        CubeGridList.Add(new($"{"BlueprintDetailPage_GridName".GetLocalized()}: {grid.DisplayName}", $"{"BlueprintDetailPage_CubeCount".GetLocalized()}: {grid.CubeBlocks.Count}"));
                    }
                TotalCubeNumber = $"{"BlueprintDetailPage_BlueprintTotalCubes".GetLocalized()}: {totalCubeCount}";
                TotalGridNumber = $"{"BlueprintDetailPage_BlueprintTotalGrids".GetLocalized()}: {currentBlueprint.CubeGrids.Length}";
            }
            else if (currentBlueprint.CubeGrid is not null)
            {
                CubeGridList.Add(new($"{"BlueprintDetailPage_GridName".GetLocalized()}: {currentBlueprint.CubeGrid.DisplayName}", $"{"BlueprintDetailPage_CubeCount".GetLocalized()}: {currentBlueprint.CubeGrid.CubeBlocks.Count}"));
            }
            else
            {
                CubeGridList.Add(new("BlueprintDetailPage_CantFindGrid".GetLocalized(), $"{"BlueprintDetailPage_CubeCount".GetLocalized()}: {"None".GetLocalized()}"));
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
            DLCList.Add("None".GetLocalized());
        }
    }

    [RelayCommand]
    void GoToEditorPage() => navigationViewService?.NavigateTo(typeof(BlueprintEditPage), new BlueprintModel()
    {
        BlueprintDefinitions = currentDefinitions,
        ViewData = currentBlueprintInfoViewData
    }, new DrillInNavigationTransitionInfo());
}
