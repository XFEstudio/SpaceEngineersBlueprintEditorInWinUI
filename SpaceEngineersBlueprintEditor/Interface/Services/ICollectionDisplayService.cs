namespace SpaceEngineersBlueprintEditor.Interface.Services;

/// <summary>
/// 集合显示服务
/// </summary>
/// <typeparam name="T">项泛型</typeparam>
public interface ICollectionDisplayService<T> where T : class
{
    /// <summary>
    /// 页面是否已经加载
    /// </summary>
    bool IsPageLoaded { get; }
    /// <summary>
    /// 选择指定的项
    /// </summary>
    /// <param name="item">指定的项</param>
    void Select(T? item);
}
