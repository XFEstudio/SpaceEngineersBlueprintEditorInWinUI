using Microsoft.UI.Xaml.Media;
using VRage.Game;

namespace SpaceEngineersBlueprintEditor.Model;

public class DefinitionViewData(ImageSource? imageSource, MyDefinitionBase myDefinitionBase)
{
    public ImageSource? ImageSource { get; set; } = imageSource;
    public MyDefinitionBase? DefinitionBase { get; set; } = myDefinitionBase;
}
