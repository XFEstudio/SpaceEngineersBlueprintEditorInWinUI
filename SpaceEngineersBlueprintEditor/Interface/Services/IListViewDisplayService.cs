namespace SpaceEngineersBlueprintEditor.Interface.Services;

/// <summary>
/// ListView显示服务
/// </summary>
/// <typeparam name="T">项泛型</typeparam>
public interface IListViewDisplayService<T> : ICollectionDisplayService<T> where T : class
{
    /// <summary>
    /// 选择项改变事件
    /// </summary>
    event SelectionChangedEventHandler SelectionChanged;
    /// <summary>
    /// 初始化ListView显示服务
    /// </summary>
    /// <param name="page">页面</param>
    /// <param name="listView">ListView控件</param>
    void Initialize(Page page, ListView listView);
}
