using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Utilities;

namespace SpaceEngineersBlueprintEditor.Implements;

public abstract class GlobalServiceBase : IGlobalService
{
    protected GlobalServiceBase() => GlobalServiceManager.RegisterGlobalService(this);
}
