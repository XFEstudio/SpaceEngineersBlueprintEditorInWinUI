using Microsoft.UI.Xaml.Media;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Windows.Foundation;
using XFEExtension.NetCore.StringExtension;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

internal class DefinitionPropertiesDisplayService<T> : IDefinitionPropertiesDisplayService<T> where T : DefinitionViewData
{
    private ItemsView? _itemsView;
    private Page? _page;
    private bool needToTrig = false;
    private int currentTrigger = 0;
    public bool IsPageLoaded => _page is not null && _page.IsLoaded;
    public event TypedEventHandler<ItemsView, ItemsViewSelectionChangedEventArgs>? SelectionChanged;

    [MemberNotNull(nameof(_page), nameof(_itemsView))]
    public void Initialize(Page page, ItemsView itemsView)
    {
        _page = page;
        _itemsView = itemsView;
        _itemsView.SelectionChanged += (sender, args) => SelectionChanged?.Invoke(sender, args); ;
        _page.Loaded += Page_Loaded;
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        if (needToTrig)
            _itemsView?.Select(currentTrigger);
        needToTrig = false;
    }

    public void Select(T item)
    {
        if (_itemsView is not null && _page is not null && _itemsView.ItemsSource is ObservableCollection<T> itemSource)
            if (_page.IsLoaded)
            {
                _itemsView.Select(itemSource.IndexOf(item));
            }
            else
            {
                needToTrig = true;
                currentTrigger = itemSource.IndexOf(item);
            }
    }
}
