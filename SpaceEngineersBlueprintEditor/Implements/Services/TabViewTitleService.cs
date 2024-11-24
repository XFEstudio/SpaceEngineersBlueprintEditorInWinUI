using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using XFEExtension.NetCore.DelegateExtension;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

internal class TabViewTitleService : GlobalServiceBase, ITabViewTitleService
{
    public event XFEEventHandler<string, BlueprintModel>? ChangeTitleRequest;

    public void SetTabViewTitle(string title, BlueprintModel blueprintModel) => ChangeTitleRequest?.Invoke(title, blueprintModel);
}
