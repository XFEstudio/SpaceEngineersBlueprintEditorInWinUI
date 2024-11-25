using Microsoft.UI.Xaml.Media;

namespace SpaceEngineersBlueprintEditor.Model;

/// <summary>
/// 蓝图信息视图数据
/// </summary>
/// <param name="blueprintImage">预览图</param>
/// <param name="noBlueprint">是否不包含蓝图文件</param>
/// <param name="name">蓝图名称</param>
/// <param name="fileSize">蓝图文件大小</param>
/// <param name="filePath">蓝图路径</param>
public class BlueprintInfoViewData(ImageSource blueprintImage, bool noBlueprint, string name, string fileSize, string filePath)
{
    /// <summary>
    /// 预览图
    /// </summary>
    public ImageSource BlueprintImage { get; set; } = blueprintImage;
    /// <summary>
    /// 是否不包含蓝图文件
    /// </summary>
    public bool NoBlueprint { get; set; } = noBlueprint;
    /// <summary>
    /// 蓝图名称
    /// </summary>
    public string Name { get; set; } = name;
    /// <summary>
    /// 蓝图文件大小
    /// </summary>
    public string FileSize { get; set; } = fileSize;
    /// <summary>
    /// 蓝图文件路径
    /// </summary>
    public string FilePath { get; set; } = filePath;
}
