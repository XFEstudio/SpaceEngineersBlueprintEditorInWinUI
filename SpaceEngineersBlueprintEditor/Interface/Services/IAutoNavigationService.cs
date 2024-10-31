using Microsoft.UI.Xaml.Navigation;

namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface IAutoNavigationService : INavigationService, IAutoNavigable
{
    event NavigatedEventHandler Navigated;
}
