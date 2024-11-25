using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Utilities;

namespace SpaceEngineersBlueprintEditor.Implements;

/// <summary>
/// 全局服务接口基类
/// </summary>
public abstract class GlobalServiceBase : IGlobalService
{
    ///<inheritdoc/>
    protected GlobalServiceBase() => GlobalServiceManager.RegisterGlobalService(this);
}
