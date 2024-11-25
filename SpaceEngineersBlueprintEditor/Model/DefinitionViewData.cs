using Microsoft.UI.Xaml.Media;
using VRage.Game;
using XFEExtension.NetCore.StringExtension;

namespace SpaceEngineersBlueprintEditor.Model;

/// <summary>
/// 定义集视图数据
/// </summary>
public class DefinitionViewData
{
    /// <summary>
    /// 定义集图片
    /// </summary>
    public ImageSource? ImageSource { get; set; }
    /// <summary>
    /// 附加信息
    /// </summary>
    public string? AdditionalInfo { get; set; }
    /// <summary>
    /// 是否有附加信息
    /// </summary>
    public bool HasAdditionalInfo => AdditionalInfo is not null && !AdditionalInfo.IsNullOrEmpty();
    /// <summary>
    /// 方块大小
    /// </summary>
    public string? CubeSize { get; set; }
    /// <summary>
    /// 是否有方块大小信息
    /// </summary>
    public bool HasCubeSize => CubeSize is not null && !CubeSize.IsNullOrEmpty();
    /// <summary>
    /// 定义集
    /// </summary>
    public required MyDefinitionBase DefinitionBase { get; set; }
}
