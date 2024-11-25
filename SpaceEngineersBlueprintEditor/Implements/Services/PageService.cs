using SpaceEngineersBlueprintEditor.Interface.Services;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

/// <inheritdoc cref="IPageService"/>
internal class PageService : IPageService
{
    private Page? _page;
    public Page? CurrentPage => _page;

    public void Initialize(Page page) => _page = page;
}
