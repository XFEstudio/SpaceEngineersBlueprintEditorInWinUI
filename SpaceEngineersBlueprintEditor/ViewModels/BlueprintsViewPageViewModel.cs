using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class BlueprintsViewPageViewModel : ViewModelBase
{
    private string currentParameter = "";
    private BlueprintInfoViewData? currentBlueprintInfoViewData;
    private readonly IMessageService? messageService = GlobalServiceManager.GetService<IMessageService>();
    [ObservableProperty]
    private string searchText = "";
    [ObservableProperty]
    private string copyFolderText = "";
    [ObservableProperty]
    private string deleteEnsureText = "";
    public INavigationParameterService<object> NavigationParameterService { get; set; } = new NavigationParameterService<object>();
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

    private void LoadBlueprints(List<BlueprintInfo> blueprintInfoList)
    {
        foreach (var info in blueprintInfoList)
            BlueprintInfoViewDataList.Add(info.ToBlueprintInfoViewData());
    }

    private async void NavigationParameterService_ParameterChange(object? sender, object e)
    {
        if (e is string stringParameter)
        {
            currentParameter = stringParameter;
            await BlueprintsManager.LoadBlueprintsAsync();
            BlueprintInfoViewDataList.Clear();
            LoadCurrentBlueprints();
        }
        else if (e is BlueprintInfoViewData blueprintInfoViewData && Path.GetDirectoryName(blueprintInfoViewData.FilePath) is string path)
        {
            currentBlueprintInfoViewData = blueprintInfoViewData;
            DeleteEnsureText = $"Are you sure to delete {blueprintInfoViewData.Name}?";
            CopyFolderText = Path.GetFileName(FileHelper.GetCopyFileName(path)) ?? throw new NullReferenceException("Can't get the folder name");
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
        messageService?.ShowMessage("刷新成功", "完成", InfoBarSeverity.Success);
    }

    [RelayCommand]
    void OpenFolder()
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
        if (currentBlueprintInfoViewData is not null && Path.GetDirectoryName(currentBlueprintInfoViewData.FilePath) is string path && await BlueprintsViewPage.Current?.copyToContentDialog.ShowAsync() == ContentDialogResult.Primary)
        {
            var targetPath = $@"{Path.GetDirectoryName(path)}\{CopyFolderText}";
            FileHelper.CopyDirectory(path, targetPath);
            messageService?.ShowMessage($"已复制至：{targetPath}", "完成", InfoBarSeverity.Success);
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
            messageService?.ShowMessage($"已删除：{path}", "完成", InfoBarSeverity.Success);
            BlueprintsViewPage.Current?.commandBarFlyout.Hide();
            await RefreshBlueprints();
        }
    }
}
