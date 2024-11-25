using SpaceEngineersBlueprintEditor.Interface.Services;
using Windows.Foundation;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

/// <inheritdoc cref="ISelectorBarService"/>
internal class SelectorBarService : ISelectorBarService
{
    public event TypedEventHandler<SelectorBar, SelectorBarSelectionChangedEventArgs>? SelectionChanged;
    public void Initialize(SelectorBar selectorBar) => selectorBar.SelectionChanged += (sender, args) => SelectionChanged?.Invoke(sender, args);
}
