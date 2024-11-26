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
    public ITabViewTitleService TabViewTitleService { get; set; } = new TabViewTitleService();
    public INavigationParameterService<BlueprintModel?> NavigationParameterService { get; set; } = new NavigationParameterService<BlueprintModel?>();
    public BlueprintEditPageViewModel()
    {
        NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;
        TabViewTitleService.ChangeTitleRequest += TabViewTitleService_ChangeTitleRequest;
    }

    private void TabViewTitleService_ChangeTitleRequest(string sender, BlueprintModel e)
    {
        if (GetCurrentItem(e) is TabViewItem tabViewItem)
            tabViewItem.Header = sender;
    }

    partial void OnSelectedTabViewItemChanged(TabViewItem? value)
    {
        if (CurrentSelectedModel is not null && CurrentSelectedModel.ViewData is not null)
            BackgroundImageService?.SetBackgroundImage(CurrentSelectedModel.ViewData.BlueprintImage);
        else
            BackgroundImageService?.ResetBackground();
    }

    private TabViewItem? GetCurrentItem(BlueprintModel blueprintModel)
    {
        foreach (var tabViewItem in TabViewItems)
            if (tabViewItem.Content is Frame frame && frame.Content is BlueprintEditSubPage blueprintEditSubPage && blueprintEditSubPage.Parameter == blueprintModel)
                return tabViewItem;
        return null;
    }

    private void NavigationParameterService_ParameterChange(object? sender, BlueprintModel? e)
    {
        if (e is not null)
        {
            if (GetCurrentItem(e) is TabViewItem tabViewItem)
                SelectedTabViewItem = tabViewItem;
            else
                CreateTabViewItem(e);
        }
        else if (e is null && TabViewItems.Count == 0)
        {
            CreateTabViewItem(null);
        }
    }

    public void CreateTabViewItem(BlueprintModel? blueprintModel)
    {
        var frame = new Frame();
        var tabView = new TabViewItem
        {
            Header = "BlueprintEditPage_NewBlueprintEditPage".GetLocalized(),
            Content = frame
        };
        TabViewItems.Add(tabView);
        SelectedTabViewItem = tabView;
        frame.Navigate(typeof(BlueprintEditSubPage), blueprintModel);
    }

    [RelayCommand]
    void AddNewTabViewItem() => CreateTabViewItem(null);
}
