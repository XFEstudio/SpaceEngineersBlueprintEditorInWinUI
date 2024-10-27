namespace SpaceEngineersBlueprintEditor.Utilities;

public static class PageManager
{
    public static Dictionary<string, Type> PageDefinitions { get; set; } = [];
    public static Dictionary<string, Page> CurrentPages { get; set; } = [];
    public static bool RegisterPage(Type pageType) => PageDefinitions.TryAdd(pageType.FullName!, pageType);
    public static bool AddOrUpdateCurrentPage<T>(T page) where T : Page
    {
        var pageName = page.GetType().FullName!;
        if (CurrentPages.TryAdd(pageName, page))
            return true;
        CurrentPages[pageName] = page;
        return false;
    }
}
