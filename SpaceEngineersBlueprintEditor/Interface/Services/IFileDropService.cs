using Microsoft.UI.Composition;

namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface IFileDropService
{
    event EventHandler<(string, DragEventArgs)> Drop;
    void Initialize(UIElement element, Compositor compositor);
}
