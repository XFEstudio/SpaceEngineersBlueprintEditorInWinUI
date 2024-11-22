namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface ITreeViewService
{
    TreeView? TreeView { get; protected set; }
    void Initialize(TreeView treeView) => TreeView = treeView;
    void Add(TreeViewNode node) => TreeView?.RootNodes.Add(node);
    void Clear() => TreeView?.RootNodes.Clear();
}
