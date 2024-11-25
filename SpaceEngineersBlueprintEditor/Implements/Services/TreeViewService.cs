using SpaceEngineersBlueprintEditor.Interface.Services;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

/// <inheritdoc cref="ITreeViewService"/>
public class TreeViewService : ITreeViewService
{
    public TreeView? TreeView { get; set; }
}
