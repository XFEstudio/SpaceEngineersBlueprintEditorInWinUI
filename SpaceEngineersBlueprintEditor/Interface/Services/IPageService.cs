namespace SpaceEngineersBlueprintEditor.Interface.Services;

/// <summary>
/// 页面服务
/// </summary>
public interface IPageService
{
    /// <summary>
    /// 当前页面
    /// </summary>
    Page? CurrentPage { get; }
    /// <summary>
    /// 初始化页面服务
    /// </summary>
    /// <param name="page">当前页面</param>
    void Initialize(Page page);
}
