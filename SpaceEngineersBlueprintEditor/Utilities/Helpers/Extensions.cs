using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.Windows.ApplicationModel.Resources;
using SpaceEngineersBlueprintEditor.Model;

namespace SpaceEngineersBlueprintEditor.Utilities.Helpers;

/// <summary>
/// 扩展类
/// </summary>
public static class Extensions
{
    private static readonly ResourceLoader _resourceLoader = new();
    /// <summary>
    /// 获取本地化文本
    /// </summary>
    /// <param name="resourceKey"></param>
    /// <returns></returns>
    public static string GetLocalized(this string resourceKey) => _resourceLoader.GetString(resourceKey);
    /// <summary>
    /// 转化为蓝图信息视图数据
    /// </summary>
    /// <param name="blueprintInfo">蓝图信息</param>
    /// <returns></returns>
    public static BlueprintInfoViewData ToBlueprintInfoViewData(this BlueprintInfo blueprintInfo) => new(new BitmapImage(blueprintInfo.HasImage ? new(blueprintInfo.ImagePath) : new Uri("ms-appx:///Assets/Images/BlueprintDrag.png", UriKind.RelativeOrAbsolute)), !blueprintInfo.HasBlueprint, blueprintInfo.Name, blueprintInfo.FileSize, blueprintInfo.BlueprintPath);
}
