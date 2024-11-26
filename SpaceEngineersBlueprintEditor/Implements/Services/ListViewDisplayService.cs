using SpaceEngineersBlueprintEditor.Interface.Services;
using System.Diagnostics.CodeAnalysis;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

internal class ListViewDisplayService<T> : IListViewDisplayService<T> where T : class
{
    private ListView? _listView;
    private Page? _page;
    public bool IsPageLoaded => _page is not null && _page.IsLoaded;
    public event SelectionChangedEventHandler? SelectionChanged;

    [MemberNotNull(nameof(_page), nameof(_listView))]
    public void Initialize(Page page, ListView listView)
    {
        _page = page;
        _listView = listView;
        _listView.SelectionChanged += (sender, e) => SelectionChanged?.Invoke(sender, e);
    }

    public void Select(T? item)
    {
        if (_listView is not null && _page is not null && _page.IsLoaded)
            _listView.SelectedItem = item;
    }
}
