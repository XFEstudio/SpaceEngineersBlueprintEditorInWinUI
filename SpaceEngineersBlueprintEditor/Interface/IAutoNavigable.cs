using Microsoft.UI.Xaml.Media.Animation;

namespace SpaceEngineersBlueprintEditor.Interface;

/// <summary>
/// 自动导航服务
/// </summary>
public interface IAutoNavigable
{
    /// <summary>
    /// 导航到目标页面
    /// </summary>
    /// <param name="pageType">目标页面名称</param>
    /// <param name="parameter">导航参数</param>
    /// <param name="navigationTransitionInfo">导航转换信息</param>
    void NavigateTo(string pageType, object? parameter = null, NavigationTransitionInfo? navigationTransitionInfo = null);
    /// <summary>
    /// 导航到目标页面
    /// </summary>
    /// <param name="type">目标页面类型</param>
    /// <param name="parameter">导航参数</param>
    /// <param name="navigationTransitionInfo">导航转换信息</param>
    void NavigateTo(Type type, object? parameter = null, NavigationTransitionInfo? navigationTransitionInfo = null);
    /// <summary>
    /// 导航到目标页面
    /// </summary>
    /// <typeparam name="T">目标页面泛型</typeparam>
    /// <param name="parameter">导航参数</param>
    /// <param name="navigationTransitionInfo">导航转换信息</param>
    void NavigateTo<T>(object? parameter = null, NavigationTransitionInfo? navigationTransitionInfo = null) where T : Page;
}