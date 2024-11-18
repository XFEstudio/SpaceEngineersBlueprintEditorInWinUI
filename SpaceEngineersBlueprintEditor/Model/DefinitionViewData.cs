using Microsoft.UI.Xaml.Media;
using VRage.Game;
using XFEExtension.NetCore.StringExtension;

namespace SpaceEngineersBlueprintEditor.Model;

public class DefinitionViewData
{
    public ImageSource? ImageSource { get; set; }
    public string? AdditionalInfo { get; set; }
    public bool HasAdditionalInfo => AdditionalInfo is not null && !AdditionalInfo.IsNullOrEmpty();
    public string? CubeSize { get; set; }
    public bool HasCubeSize => CubeSize is not null && !CubeSize.IsNullOrEmpty();
    public required MyDefinitionBase DefinitionBase { get; set; }
}
