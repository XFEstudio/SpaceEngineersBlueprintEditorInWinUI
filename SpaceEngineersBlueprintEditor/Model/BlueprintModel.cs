using VRage.Game;

namespace SpaceEngineersBlueprintEditor.Model;

/// <summary>
/// 蓝图模型
/// </summary>
public class BlueprintModel
{
    /// <summary>
    /// 蓝图定义集
    /// </summary>
    public MyObjectBuilder_Definitions? BlueprintDefinitions { get; set; }
    /// <summary>
    /// 蓝图文件视图数据
    /// </summary>
    public BlueprintInfoViewData? ViewData { get; set; }
}
