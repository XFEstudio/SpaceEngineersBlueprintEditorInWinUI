using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.Windows.ApplicationModel.Resources;
using SpaceEngineersBlueprintEditor.Model;
using XFEExtension.NetCore.FileExtension;

namespace SpaceEngineersBlueprintEditor.Utilities.Helpers;

public static class Extensions
{
    private static readonly ResourceLoader _resourceLoader = new();
    public static string GetLocalized(this string resourceKey) => _resourceLoader.GetString(resourceKey);
    public static BlueprintInfoViewData ToBlueprintInfoViewData(this BlueprintInfo blueprintInfo) => new(new BitmapImage(blueprintInfo.HasImage ? new(blueprintInfo.ImagePath) : new Uri("ms-appx:///Assets/Images/BlueprintDrag.png", UriKind.RelativeOrAbsolute)), !blueprintInfo.HasBlueprint, blueprintInfo.Name, blueprintInfo.FileSize, blueprintInfo.BlueprintPath);
}
