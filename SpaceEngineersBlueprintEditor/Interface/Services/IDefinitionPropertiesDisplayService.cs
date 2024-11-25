using SpaceEngineersBlueprintEditor.Model;
using Windows.Foundation;

namespace SpaceEngineersBlueprintEditor.Interface.Services;

/// <summary>
/// 定义属性显示服务
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDefinitionPropertiesDisplayService<T> where T : DefinitionViewData
{
    /// <summary>
    /// 页面是否已经加载
    /// </summary>
    bool IsPageLoaded { get; }
    /// <summary>
    /// 选择的定义改变时触发
    /// </summary>
    event TypedEventHandler<ItemsView, ItemsViewSelectionChangedEventArgs> SelectionChanged;
    /// <summary>
    /// 初始化定义属性显示服务
    /// </summary>
    /// <param name="page">显示页面</param>
    /// <param name="itemsView">显示源</param>
    void Initialize(Page page, ItemsView itemsView);
    /// <summary>
    /// 选择指定的项
    /// </summary>
    /// <param name="item">指定的项</param>
    void Select(T item);
}
