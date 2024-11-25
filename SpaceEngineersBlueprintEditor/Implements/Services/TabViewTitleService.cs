using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;
using XFEExtension.NetCore.DelegateExtension;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

/// <inheritdoc cref="ITabViewTitleService"/>
internal class TabViewTitleService : GlobalServiceBase, ITabViewTitleService
{
    public event XFEEventHandler<string, BlueprintModel>? ChangeTitleRequest;

    public void SetTabViewTitle(string title, BlueprintModel blueprintModel) => ChangeTitleRequest?.Invoke(title, blueprintModel);
}
