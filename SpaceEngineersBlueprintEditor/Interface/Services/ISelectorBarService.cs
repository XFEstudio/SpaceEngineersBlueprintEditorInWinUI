using Windows.Foundation;

namespace SpaceEngineersBlueprintEditor.Interface.Services;

/// <summary>
/// 选项栏服务
/// </summary>
public interface ISelectorBarService
{
    /// <summary>
    /// 选项栏选项改变事件
    /// </summary>
    event TypedEventHandler<SelectorBar, SelectorBarSelectionChangedEventArgs> SelectionChanged;
    /// <summary>
    /// 初始化选项栏服务
    /// </summary>
    /// <param name="selectorBar">选项栏控件</param>
    void Initialize(SelectorBar selectorBar);
}
