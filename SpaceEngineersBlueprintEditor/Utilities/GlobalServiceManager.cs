using SpaceEngineersBlueprintEditor.Interface.Services;

namespace SpaceEngineersBlueprintEditor.Utilities;

/// <summary>
/// 全局服务管理器
/// </summary>
public static class GlobalServiceManager
{
    private static readonly List<IGlobalService> services = [];
    /// <summary>
    /// 注册全局服务
    /// </summary>
    /// <param name="service">全局服务</param>
    public static void RegisterGlobalService(IGlobalService service) => services.Add(service);
    /// <summary>
    /// 获取全局服务
    /// </summary>
    /// <typeparam name="T">全局服务泛型</typeparam>
    /// <returns></returns>
    public static T? GetService<T>() where T : IGlobalService => services.OfType<T>().FirstOrDefault();
}
