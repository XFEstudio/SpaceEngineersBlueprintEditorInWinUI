using SpaceEngineersBlueprintEditor.Interface.Services;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Windows.Foundation;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

/// <inheritdoc cref="IItemsViewDisplayService{T}"/>
internal class ItemsViewDisplayService<T> : IItemsViewDisplayService<T> where T : class
{
    private ItemsView? _itemsView;
    private Page? _page;
    public bool IsPageLoaded => _page is not null && _page.IsLoaded;
    public event TypedEventHandler<ItemsView, ItemsViewSelectionChangedEventArgs>? SelectionChanged;

    [MemberNotNull(nameof(_page), nameof(_itemsView))]
    public void Initialize(Page page, ItemsView itemsView)
    {
        _page = page;
        _itemsView = itemsView;
        _itemsView.SelectionChanged += (sender, args) => SelectionChanged?.Invoke(sender, args);
    }

    public void Select(T? item)
    {
        if (_itemsView is not null && _page is not null && _itemsView.ItemsSource is ObservableCollection<T> itemSource && _page.IsLoaded)
            _itemsView.Select(item is not null ? itemSource.IndexOf(item) : 0);
    }
}
