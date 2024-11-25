namespace SpaceEngineersBlueprintEditor.Model;

/// <summary>
/// 蓝图信息
/// </summary>
/// <param name="imagePath">图片路径</param>
/// <param name="hasBlueprint">是否有蓝图文件</param>
/// <param name="hasImage">是否有预览图片</param>
/// <param name="name">蓝图名称</param>
/// <param name="fileSize">蓝图文件大小</param>
/// <param name="blueprintPath">蓝图路径</param>
public class BlueprintInfo(string imagePath, bool hasBlueprint, bool hasImage, string name, string fileSize, string blueprintPath)
{
    /// <summary>
    /// 图片路径
    /// </summary>
    public string ImagePath { get; set; } = imagePath;
    /// <summary>
    /// 是否有蓝图
    /// </summary>
    public bool HasBlueprint { get; set; } = hasBlueprint;
    /// <summary>
    /// 是否有预览图片
    /// </summary>
    public bool HasImage { get; set; } = hasImage;
    /// <summary>
    /// 蓝图名称
    /// </summary>
    public string Name { get; set; } = name;
    /// <summary>
    /// 蓝图文件大小
    /// </summary>
    public string FileSize { get; set; } = fileSize;
    /// <summary>
    /// 蓝图路径
    /// </summary>
    public string BlueprintPath { get; set; } = blueprintPath;
}