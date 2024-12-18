﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
using XFEExtension.NetCore.StringExtension;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class BlueprintsViewPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private string searchText = "";
    [ObservableProperty]
    private string copyFolderText = "";
    [ObservableProperty]
    private string deleteEnsureText = "";
    private string currentParameter = "";
    private BlueprintInfoViewData? currentBlueprintInfoViewData;
    private readonly IMessageService? messageService = GlobalServiceManager.GetService<IMessageService>();
    public INavigationParameterService<object> NavigationParameterService { get; set; } = new NavigationParameterService<object>();
    public IDialogService DialogService { get; set; } = new DialogService();
    public ObservableCollection<BlueprintInfoViewData> BlueprintInfoViewDataList { get; set; } = [];

    public BlueprintsViewPageViewModel()
    {
        NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;
    }

    partial void OnSearchTextChanged(string value)
    {
        BlueprintInfoViewDataList.Clear();
        LoadBlueprints(SearchBlueprints(currentParameter, value).ToList());
    }

    private static IEnumerable<BlueprintInfo> SearchBlueprints(string searchMode, string blueprintName) => GetCurrentBlueprints(searchMode).Where(blueprint => blueprint.Name.Contains(blueprintName, StringComparison.OrdinalIgnoreCase));

    private static List<BlueprintInfo> GetCurrentBlueprints(string currentLocation) => currentLocation switch
    {
        "Local" => BlueprintsManager.LocalBlueprints,
        "Cloud" => BlueprintsManager.CloudBlueprints,
        "Workshop" => BlueprintsManager.WorkshopBlueprints,
        _ => throw new NotImplementedException()
    };

    private void LoadCurrentBlueprints() => LoadBlueprints(GetCurrentBlueprints(currentParameter));

    private void LoadBlueprints(List<BlueprintInfo> blueprintInfoList) => blueprintInfoList.ForEach(info => BlueprintInfoViewDataList.Add(info.ToBlueprintInfoViewData()));

    private async void NavigationParameterService_ParameterChange(object? sender, object? e)
    {
        if (e is string stringParameter && !currentParameter.Equals(stringParameter))
        {
            currentParameter = stringParameter;
            await BlueprintsManager.LoadBlueprintsAsync();
            BlueprintInfoViewDataList.Clear();
            if (SearchText.IsNullOrEmpty())
                LoadCurrentBlueprints();
            else
                LoadBlueprints(SearchBlueprints(currentParameter, SearchText).ToList());
        }
        else if (e is BlueprintInfoViewData blueprintInfoViewData && Path.GetDirectoryName(blueprintInfoViewData.FilePath) is string path)
        {
            currentBlueprintInfoViewData = blueprintInfoViewData;
            DeleteEnsureText = $"{"BlueprintViewPage_EnsureToDelete".GetLocalized()} {blueprintInfoViewData.Name}?";
            CopyFolderText = Path.GetFileName(FileHelper.GetCopyDirectoryName(path)) ?? throw new NullReferenceException("Can't get the folder name");
        }
    }

    [RelayCommand]
    async Task RefreshBlueprints()
    {
        await BlueprintsManager.LoadBlueprintsAsync();
        if (SearchText == string.Empty)
            LoadCurrentBlueprints();
        else
            OnSearchTextChanged(SearchText);
        messageService?.ShowMessage("RefreshComplete".GetLocalized(), "Complete".GetLocalized(), InfoBarSeverity.Success);
    }

    [RelayCommand]
    void OpenInFolder()
    {
        if (currentBlueprintInfoViewData is not null && Path.GetDirectoryName(currentBlueprintInfoViewData.FilePath) is string path)
        {
            Process.Start("explorer.exe", path);
            BlueprintsViewPage.Current?.commandBarFlyout.Hide();
        }
    }

    [RelayCommand]
    async Task CopyTo()
    {
        if (currentBlueprintInfoViewData is not null && Path.GetDirectoryName(currentBlueprintInfoViewData.FilePath) is string path && await DialogService.ShowDialog("copyToContentDialog") == ContentDialogResult.Primary)
        {
            var targetPath = $@"{Path.GetDirectoryName(path)}\{CopyFolderText}";
            FileHelper.CopyDirectory(path, targetPath);
            messageService?.ShowMessage($"{"CopyToComplete".GetLocalized()}: {targetPath}", "Complete".GetLocalized(), InfoBarSeverity.Success);
            BlueprintsViewPage.Current?.commandBarFlyout.Hide();
            await RefreshBlueprints();
        }
    }

    [RelayCommand]
    async Task DeleteBlueprint()
    {
        if (currentBlueprintInfoViewData is not null && Path.GetDirectoryName(currentBlueprintInfoViewData.FilePath) is string path)
        {
            Directory.Delete(path, true);
            messageService?.ShowMessage($"{"DeleteAtComplete".GetLocalized()}: {path}", "Complete".GetLocalized(), InfoBarSeverity.Success);
            BlueprintsViewPage.Current?.commandBarFlyout.Hide();
            await RefreshBlueprints();
        }
    }
}
