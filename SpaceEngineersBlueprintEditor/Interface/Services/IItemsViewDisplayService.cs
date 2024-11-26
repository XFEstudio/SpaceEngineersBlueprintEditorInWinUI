using Windows.Foundation;

namespace SpaceEngineersBlueprintEditor.Interface.Services;

/// <summary>
/// ItemsView显示服务
/// </summary>
/// <typeparam name="T">项泛型</typeparam>
public interface IItemsViewDisplayService<T> : ICollectionDisplayService<T> where T : class
{
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
}
