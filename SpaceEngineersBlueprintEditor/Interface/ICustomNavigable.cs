namespace SpaceEngineersBlueprintEditor.Interface;

/// <summary>
/// 自定义导航服务
/// </summary>
public interface ICustomNavigable
{
    /// <summary>
    /// 导航到目标页面
    /// </summary>
    /// <typeparam name="T">目标页面泛型</typeparam>
    /// <param name="parameter">导航参数</param>
    void NavigateTo<T>(object? parameter = null) where T : Page;
    /// <summary>
    /// 导航到目标页面
    /// </summary>
    /// <param name="type">目标页面类型</param>
    /// <param name="parameter">导航参数</param>
    void NavigateTo(Type type, object? parameter = null, bool goBack = false);
    /// <summary>
    /// 导航到目标页面
    /// </summary>
    /// <param name="pageName">目标页面名称</param>
    /// <param name="parameter">导航参数</param>
    void NavigateTo(string pageName, object? parameter = null);
}
