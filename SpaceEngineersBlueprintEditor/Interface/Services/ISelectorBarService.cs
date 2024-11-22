using Windows.Foundation;

namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface ISelectorBarService
{
    event TypedEventHandler<SelectorBar, SelectorBarSelectionChangedEventArgs> SelectionChanged;
    void Initialize(SelectorBar selectorBar);
}
