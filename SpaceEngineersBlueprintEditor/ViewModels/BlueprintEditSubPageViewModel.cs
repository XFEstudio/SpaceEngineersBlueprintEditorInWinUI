﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
using VRage.Game;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class BlueprintEditSubPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool isCubeGridListVisible;
    [ObservableProperty]
    private bool isShipBlueprintPropertyVisible;
    [ObservableProperty]
    private bool isContentGridVisible = false;
    [ObservableProperty]
    private bool isInitialGridVisible = true;
    [ObservableProperty]
    private string searchText = string.Empty;
    [ObservableProperty]
    private TreeViewNode? selectedTreeViewNode;
    private BlueprintModel? currentBlueprintModel;
    private MyObjectBuilder_Definitions? currentDefinitions;
    private MyObjectBuilder_ShipBlueprintDefinition? currentShipBlueprint;
    private readonly ILoadingService? loadingService = GlobalServiceManager.GetService<ILoadingService>();
    private readonly IMessageService? messageService = GlobalServiceManager.GetService<IMessageService>();
    private readonly INavigationViewService? navigationViewService = GlobalServiceManager.GetService<INavigationViewService>();
    private readonly ITabViewTitleService? tabViewTitleService = GlobalServiceManager.GetService<ITabViewTitleService>();
    public ObservableCollection<BlueprintGroupList> BlueprintCubeGridList { get; } = [];
    public IBackgroundImageService? BackgroundImageService { get; set; } = GlobalServiceManager.GetService<IBackgroundImageService>();
    public INavigationParameterService<BlueprintModel> NavigationParameterService { get; set; } = new NavigationParameterService<BlueprintModel>();
    public IFileDropService FileDropService { get; set; } = new BlueprintDropService();
    public ITreeViewService TreeViewService { get; set; } = new TreeViewService();
    public ISelectorBarService SelectorBarService { get; set; } = new SelectorBarService();

    public BlueprintEditSubPageViewModel()
    {
        NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;
        SelectorBarService.SelectionChanged += SelectorBarService_SelectionChanged;
        FileDropService.Drop += FileDropService_Drop;
    }

    private async void FileDropService_Drop(object? sender, (string, DragEventArgs) e)
    {
        loadingService?.StartLoading("Loading definitions...");
        if (File.Exists(e.Item1) && await SpaceEngineersHelper.LoadBlueprintModel(e.Item1) is BlueprintModel blueprintModel)
        {
            NavigationParameterService.Parameter = blueprintModel;
            if (blueprintModel.ViewData is not null)
                tabViewTitleService?.SetTabViewTitle(blueprintModel.ViewData.Name, blueprintModel);
            await SetDefinitions(blueprintModel.BlueprintDefinitions);
        }
        loadingService?.StopLoading();
    }

    partial void OnSearchTextChanged(string value) => SearchCubeGrids(value);

    private async void SelectorBarService_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args) => await LoadByName(sender.SelectedItem.Text);

    private async Task SetDefinitions(MyObjectBuilder_Definitions? definitions)
    {
        if (definitions is not null)
        {
            currentDefinitions = definitions;
            if (currentDefinitions.ShipBlueprints is not null && currentDefinitions.ShipBlueprints.Length > 0)
            {
                currentShipBlueprint = currentDefinitions.ShipBlueprints[0];
                IsContentGridVisible = true;
                IsInitialGridVisible = false;
                await Task.Delay(200);
                await LoadByName("Grids");
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
    }

    private async void NavigationParameterService_ParameterChange(object? sender, BlueprintModel? e)
    {
        if (e is null || NavigationParameterService.SameAsLast)
            return;
        currentBlueprintModel = e;
        if (currentBlueprintModel.ViewData is not null)
            BackgroundImageService?.SetBackgroundImage(currentBlueprintModel.ViewData.BlueprintImage);
        if (navigationViewService is not null) navigationViewService.Header = null;
        if (e.ViewData is not null)
            tabViewTitleService?.SetTabViewTitle(e.ViewData.Name, e);
        if (e.BlueprintDefinitions is not null)
        {
            await SetDefinitions(e.BlueprintDefinitions);
        }
        else if (e.ViewData is not null)
        {
            await SetDefinitions(await SpaceEngineersHelper.LoadBlueprintAsync(e.ViewData.FilePath));
        }
    }

    private async Task LoadByName(string caseName)
    {
        if (currentShipBlueprint is null) return;
        loadingService?.StartLoading("Loading definitions...");
        await Task.Delay(50);
        switch (caseName)
        {
            case "Grids":
                IsCubeGridListVisible = true;
                IsShipBlueprintPropertyVisible = false;
                LoadCubeGrids();
                break;
            case "Properties":
                IsCubeGridListVisible = false;
                IsShipBlueprintPropertyVisible = true;
                LoadBlueprintPropertyDefinitions();
                break;
            default:
                break;
        }
        loadingService?.StopLoading();
    }

    private void LoadBlueprintPropertyDefinitions()
    {
        TreeViewService.Clear();
        var parent = new BlueprintPropertyViewData
        {
            Value = currentShipBlueprint,
            Name = "飞船蓝图",
            Type = currentShipBlueprint!.GetType()
        };
        SpaceEngineersHelper.AnalyzeBlueprint(parent);
        foreach (var child in parent.Children)
        {
            TreeViewService.Add(new TreeViewNode
            {
                Content = child,
                HasUnrealizedChildren = !child.IsBasicType
            });
        }
    }

    private void SearchCubeGrids(string name)
    {
        BlueprintCubeGridList.Clear();
        currentShipBlueprint!.CubeGrids.Select(grid =>
        {
            var type = grid.CubeBlocks.GetType();
            var gridViewData = new BlueprintPropertyViewData
            {
                Name = type.Name,
                Type = type,
                Value = grid.CubeBlocks
            };
            SpaceEngineersHelper.AnalyzeBlueprint(gridViewData);
            var targetChild = gridViewData.Children.Where(child => (child.Name is not null && child.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) || (child.CustomData is string customName && customName.Contains(name, StringComparison.OrdinalIgnoreCase)));
            if (!targetChild.Any())
                return null;
            return new BlueprintGroupList(targetChild)
            {
                GroupName = grid.DisplayName
            };
        }).Where(grid => grid is not null).ForEach(BlueprintCubeGridList.Add!);
    }

    private void LoadCubeGrids()
    {
        BlueprintCubeGridList.Clear();
        currentShipBlueprint!.CubeGrids.Select(grid =>
        {
            var type = grid.CubeBlocks.GetType();
            var gridViewData = new BlueprintPropertyViewData
            {
                Name = type.Name,
                Type = type,
                Value = grid.CubeBlocks
            };
            SpaceEngineersHelper.AnalyzeBlueprint(gridViewData);
            return new BlueprintGroupList(gridViewData.Children)
            {
                GroupName = grid.DisplayName
            };
        }).ForEach(BlueprintCubeGridList.Add);
    }

    [RelayCommand]
    async Task OpenBlueprint()
    {
        loadingService?.StartLoading("Loading definitions...");
        var openPicker = new FileOpenPicker();
        InitializeWithWindow.Initialize(openPicker, WindowNative.GetWindowHandle(App.MainWindow));
        openPicker.ViewMode = PickerViewMode.List;
        openPicker.FileTypeFilter.Add(".sbc");
        if (await openPicker.PickSingleFileAsync() is StorageFile file && File.Exists(file.Path))
        {
            if (await SpaceEngineersHelper.LoadBlueprintModel(file.Path) is BlueprintModel blueprintModel)
            {
                NavigationParameterService.Parameter = blueprintModel;
                if (blueprintModel.ViewData is not null)
                    tabViewTitleService?.SetTabViewTitle(blueprintModel.ViewData.Name, blueprintModel);
                await SetDefinitions(blueprintModel.BlueprintDefinitions);
            }
        }
        loadingService?.StopLoading();
    }

    [RelayCommand]
    void OpenInFolder()
    {
        if (currentBlueprintModel is not null && currentBlueprintModel.ViewData is not null)
            Process.Start("explorer.exe", Path.GetDirectoryName(currentBlueprintModel.ViewData.FilePath) ?? string.Empty);
        else
            messageService?.ShowMessage("无法找到文件", "警告", InfoBarSeverity.Warning);
    }

    [RelayCommand]
    void ViewBlueprintsList() => navigationViewService?.NavigateTo<BlueprintsViewPage>("Local");

    [RelayCommand]
    async Task Save()
    {
        loadingService?.StartLoading("Saving blueprint...");
        var savePicker = new FileSavePicker();
        InitializeWithWindow.Initialize(savePicker, WindowNative.GetWindowHandle(App.MainWindow));
        savePicker.FileTypeChoices.Add(new("Blueprint file", [".sbc"]));
        savePicker.SuggestedSaveFile = await StorageFile.GetFileFromPathAsync($"{NavigationParameterService.Parameter?.ViewData?.FilePath}");
        savePicker.SuggestedFileName = "bp.sbc";
        savePicker.DefaultFileExtension = ".sbc";
        if (await savePicker.PickSaveFileAsync() is StorageFile file)
        {
            try
            {
                var cachePath = @$"{AppPath.AppCache}\bp";
                var blueprintCachePath = @$"{AppPath.AppCache}\bp.sbc";
                var process = new Process();
                process.StartInfo.FileName = @"Converter\SpaceEngineersBlueprintEditor.BlueprintConverter.exe";
                File.WriteAllText(cachePath, JsonConvert.SerializeObject(currentDefinitions, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                }));
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.ArgumentList.Add(cachePath);
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                var result = process.StandardOutput.ReadToEnd();
                if (!string.IsNullOrEmpty(result))
                {
                    if (result == "Successful")
                    {
                        if (File.Exists(cachePath))
                            File.Delete(cachePath);
                        if (File.Exists(blueprintCachePath))
                        {
                            File.Move(blueprintCachePath, file.Path, true);
                            var sbcPath = $@"{Path.GetDirectoryName(file.Path)}\bp.sbcB5";
                            if (File.Exists(sbcPath))
                                File.Delete(sbcPath);
                        }
                        messageService?.ShowMessage("保存成功", "完成", InfoBarSeverity.Success);
                    }
                    else if (result.StartsWith("Error:"))
                    {
                        messageService?.ShowMessage(result.Replace("Error:", string.Empty), "失败", InfoBarSeverity.Error);
                    }
                }
                else
                {
                    messageService?.ShowMessage("保存失败：未知原因", "失败", InfoBarSeverity.Error);
                }
            }
            catch (Exception ex)
            {
                messageService?.ShowMessage($"保存失败：{ex.Message}", "失败", InfoBarSeverity.Error);
            }
        }
        loadingService?.StopLoading();
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
