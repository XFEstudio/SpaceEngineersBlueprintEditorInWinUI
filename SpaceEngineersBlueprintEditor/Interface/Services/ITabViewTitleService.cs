using SpaceEngineersBlueprintEditor.Model;
using XFEExtension.NetCore.DelegateExtension;

namespace SpaceEngineersBlueprintEditor.Interface.Services;

/// <summary>
/// 全局Tab视图标题服务
/// </summary>
public interface ITabViewTitleService : IGlobalService
{
    /// <summary>
    /// 更改标题请求事件
    /// </summary>
    event XFEEventHandler<string, BlueprintModel> ChangeTitleRequest;
    /// <summary>
    /// 设置Tab视图控件标题
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="blueprintModel">蓝图模型</param>
    void SetTabViewTitle(string title, BlueprintModel blueprintModel);
}
