using SpaceEngineersBlueprintEditor.Model;
using XFEExtension.NetCore.DelegateExtension;

namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface ITabViewTitleService : IGlobalService
{
    event XFEEventHandler<string, BlueprintModel> ChangeTitleRequest;
    void SetTabViewTitle(string title, BlueprintModel blueprintModel);
}
