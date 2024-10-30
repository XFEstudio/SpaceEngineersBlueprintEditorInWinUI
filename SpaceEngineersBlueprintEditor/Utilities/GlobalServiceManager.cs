using SpaceEngineersBlueprintEditor.Interface.Services;

namespace SpaceEngineersBlueprintEditor.Utilities;

public static class GlobalServiceManager
{
    private static List<IGlobalService> services = [];
    public static void RegisterGlobalService(IGlobalService service) => services.Add(service);
    public static T? GetService<T>() where T : IGlobalService => services.OfType<T>().FirstOrDefault();
}
