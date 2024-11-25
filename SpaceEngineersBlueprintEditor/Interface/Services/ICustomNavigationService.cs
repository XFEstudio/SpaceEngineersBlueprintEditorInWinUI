namespace SpaceEngineersBlueprintEditor.Interface.Services;

/// <summary>
/// 自定义导航服务
/// </summary>
public interface ICustomNavigationService : INavigationService, ICustomNavigable
{
    /// <summary>
    /// 导航完成
    /// </summary>
    event EventHandler<Type> Navigated;
    /// <summary>
    /// 导航堆栈
    /// </summary>
    List<(Page, object?)> NavigationStack { get; }
}
