namespace SpaceEngineersBlueprintEditor.Utilities;

/// <summary>
/// 页面管理器
/// </summary>
public static class PageManager
{
    /// <summary>
    /// 页面定义字典
    /// </summary>
    public static Dictionary<string, Type> PageDefinitions { get; set; } = [];
    /// <summary>
    /// 当前页面实例字典
    /// </summary>
    public static Dictionary<string, Page> CurrentPages { get; set; } = [];
    /// <summary>
    /// 注册页面
    /// </summary>
    /// <param name="pageType">页面类型</param>
    /// <returns>是否注册成功</returns>
    public static bool RegisterPage(Type pageType) => PageDefinitions.TryAdd(pageType.FullName!, pageType);
    /// <summary>
    /// 添加或者更新当前页面
    /// </summary>
    /// <typeparam name="T">页面泛型</typeparam>
    /// <param name="page">当前页面对象</param>
    /// <returns>添加/更新</returns>
    public static bool AddOrUpdateCurrentPage<T>(T page) where T : Page
    {
        var pageName = page.GetType().FullName!;
        if (CurrentPages.TryAdd(pageName, page))
            return true;
        CurrentPages[pageName] = page;
        return false;
    }
}
