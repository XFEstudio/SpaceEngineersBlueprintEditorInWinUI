using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.Views;
using System.Collections.ObjectModel;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class BlueprintEditPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private TabViewItem? selectedTabViewItem;
    public INavigationViewService? NavigationViewService { get; } = GlobalServiceManager.GetService<INavigationViewService>();
    public BlueprintModel? CurrentSelectedModel => SelectedTabViewItem is not null ? SelectedTabViewItem.Content is Frame frame ? frame.Content is BlueprintEditSubPage blueprintEditSubPage ? blueprintEditSubPage.Parameter : null : null : null;
    public ObservableCollection<TabViewItem> TabViewItems { get; set; } = [];
    public IBackgroundImageService? BackgroundImageService { get; set; } = GlobalServiceManager.GetService<IBackgroundImageService>();
    public INavigationParameterService<BlueprintModel?> NavigationParameterService { get; set; } = new NavigationParameterService<BlueprintModel?>();
    public BlueprintEditPageViewModel() => NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;

    partial void OnSelectedTabViewItemChanged(TabViewItem? value)
    {
        if (CurrentSelectedModel is not null && CurrentSelectedModel.ViewData is not null)
            BackgroundImageService?.SetBackgroundImage(CurrentSelectedModel.ViewData.BlueprintImage);
        else
            BackgroundImageService?.ResetBackground();
    }

    private void NavigationParameterService_ParameterChange(object? sender, BlueprintModel? e)
    {
        if (e is not null || e is null && TabViewItems.Count == 0)
            CreateTabViewItem(e);
    }

    private void CreateTabViewItem(BlueprintModel? blueprintModel)
    {
        var frame = new Frame();
        var tabView = new TabViewItem
        {
            Header = blueprintModel?.ViewData?.Name ?? "未命名蓝图",
            Content = frame
        };
        frame.Navigate(typeof(BlueprintEditSubPage), blueprintModel);
        TabViewItems.Add(tabView);
        SelectedTabViewItem = tabView;
    }

    [RelayCommand]
    void AddNewTabViewItem() => CreateTabViewItem(null);
}
