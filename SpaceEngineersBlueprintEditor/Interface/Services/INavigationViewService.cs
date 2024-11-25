namespace SpaceEngineersBlueprintEditor.Interface.Services;

/// <summary>
/// 全局导航视图服务
/// </summary>
public interface INavigationViewService : IAutoNavigable, IGlobalService
{
    /// <summary>
    /// 目标导航视图
    /// </summary>
    NavigationView? NavigationView { get; }
    /// <summary>
    /// 导航服务
    /// </summary>
    IAutoNavigationService NavigationService { get; }
    /// <summary>
    /// 初始化导航视图服务
    /// </summary>
    /// <param name="navigationView">目标导航视图</param>
    /// <param name="frame">目标导航Frame</param>
    void Initialize(NavigationView navigationView, Frame frame);
    /// <summary>
    /// 已选择的导航视图项
    /// </summary>
    object? SelectedItem { get; }
    /// <summary>
    /// 当前导航视图的标题头
    /// </summary>
    string? Header { get; set; }
    /// <summary>
    /// 导航视图内容Margin
    /// </summary>
    Thickness ContentMargin { get; set; }
    /// <summary>
    /// Xaml根节点
    /// </summary>
    XamlRoot? XamlRoot { get; }
    /// <summary>
    /// 获取当前选择的导航视图项
    /// </summary>
    /// <param name="type">目标页面类型</param>
    /// <param name="parameter">目标页面导航时的参数</param>
    /// <returns></returns>
    NavigationViewItem? GetSelectedItem(Type type, object? parameter = null);
}
