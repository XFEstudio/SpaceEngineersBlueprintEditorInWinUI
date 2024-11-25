namespace SpaceEngineersBlueprintEditor.Interface.Services;

/// <summary>
/// 树视图服务
/// </summary>
public interface ITreeViewService
{
    /// <summary>
    /// 目标树视图控件
    /// </summary>
    TreeView? TreeView { get; protected set; }
    /// <summary>
    /// 初始化树视图服务
    /// </summary>
    /// <param name="treeView">树视图控件</param>
    void Initialize(TreeView treeView) => TreeView = treeView;
    /// <summary>
    /// 添加树视图节点
    /// </summary>
    /// <param name="node">树视图节点</param>
    void Add(TreeViewNode node) => TreeView?.RootNodes.Add(node);
    /// <summary>
    /// 清除所有树视图及其子节点
    /// </summary>
    void Clear() => TreeView?.RootNodes.Clear();
}
