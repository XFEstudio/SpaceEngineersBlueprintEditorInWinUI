using Microsoft.UI.Xaml.Media.Imaging;
using SpaceEngineersBlueprintEditor.Model;

namespace SpaceEngineersBlueprintEditor.Utilities.Helpers;

public static class Extensions
{
    public static BlueprintInfoViewData ToBlueprintInfoViewData(this BlueprintInfo blueprintInfo) => new(new BitmapImage(blueprintInfo.HasImage ? new(blueprintInfo.ImagePath) : new Uri("ms-appx:///Assets/Images/BlueprintDrag.png", UriKind.RelativeOrAbsolute)), !blueprintInfo.HasBlueprint, blueprintInfo.Name, blueprintInfo.FileSize, blueprintInfo.BlueprintPath);
}
