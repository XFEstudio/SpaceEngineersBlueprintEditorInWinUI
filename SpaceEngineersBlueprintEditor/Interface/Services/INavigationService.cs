namespace SpaceEngineersBlueprintEditor.Interface.Services;

/// <summary>
/// 导航服务
/// </summary>
public interface INavigationService : IGlobalService
{
    /// <summary>
    /// 是否可以向后导航
    /// </summary>
    bool CanGoBack { get; }
    /// <summary>
    /// 是否可以向前导航
    /// </summary>
    bool CanGoForward { get; }
    /// <summary>
    /// 导航Frame控件
    /// </summary>
    Frame? Frame { get; set; }
    /// <summary>
    /// 向后导航
    /// </summary>
    void GoBack();
}
