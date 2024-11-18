using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class GameDefinitionsViewPageViewModel : ViewModelBase
{
    private string currentParameter = "";
    public INavigationParameterService<string> NavigationParameterService { get; } = new NavigationParameterService<string>();

    public GameDefinitionsViewPageViewModel()
    {
    }
}
