namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface ICustomNavigationService : INavigationService, ICustomNavigable
{
    event EventHandler<Type> Navigated;
    List<(Page, object?)> NavigationStack { get; }
}
