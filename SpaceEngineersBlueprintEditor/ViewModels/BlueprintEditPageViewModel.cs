using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Utilities;
using VRage.Game;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class BlueprintEditPageViewModel : ViewModelBase
{
    private MyObjectBuilder_Definitions? currentDefinitions;
    private MyObjectBuilder_ShipBlueprintDefinition? currentShipBlueprint;
    private readonly IMessageService? messageService = GlobalServiceManager.GetService<IMessageService>();
    public INavigationParameterService<MyObjectBuilder_Definitions> NavigationParameterService { get; set; } = new NavigationParameterService<MyObjectBuilder_Definitions>();
    public BlueprintEditPageViewModel() => NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;

    private void NavigationParameterService_ParameterChange(object? sender, MyObjectBuilder_Definitions e)
    {
        if (currentDefinitions is not null)
        {
            currentDefinitions = e;
            if (currentDefinitions.ShipBlueprints is not null && currentDefinitions.ShipBlueprints.Length > 0)
            {
                currentShipBlueprint = currentDefinitions.ShipBlueprints[0];
            }
            else
            {
                messageService?.ShowMessage("未能飞船蓝图定义", "错误", InfoBarSeverity.Error);
            }
        }
        else
        {
            messageService?.ShowMessage("未能找到蓝图定义", "错误", InfoBarSeverity.Error);
        }
    }
}
