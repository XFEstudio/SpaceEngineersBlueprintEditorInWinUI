namespace SpaceEngineersBlueprintEditor.Interface.Services;

/// <summary>
/// 导航参数服务
/// </summary>
/// <typeparam name="T">目标参数类型</typeparam>
public interface INavigationParameterService<T> : IPageService
{
    /// <summary>
    /// 是否和上一次的参数相同
    /// </summary>
    bool SameAsLast { get; }
    /// <summary>
    /// 当前导航参数
    /// </summary>
    T? Parameter { get; set; }
    /// <summary>
    /// 参数改变事件
    /// </summary>
    event EventHandler<T?> ParameterChange;
    /// <summary>
    /// 参数改变
    /// </summary>
    /// <param name="parameter">新的参数</param>
    void OnParameterChange(object? parameter);
}
