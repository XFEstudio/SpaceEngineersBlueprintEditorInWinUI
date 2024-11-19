using SpaceEngineersBlueprintEditor.Model;
using Windows.Foundation;

namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface IDefinitionPropertiesDisplayService<T> where T : DefinitionViewData
{
    bool IsPageLoaded { get; }

    event TypedEventHandler<ItemsView, ItemsViewSelectionChangedEventArgs> SelectionChanged;
    void Initialize(Page page, ItemsView itemsView);
    void Select(T item);
}
