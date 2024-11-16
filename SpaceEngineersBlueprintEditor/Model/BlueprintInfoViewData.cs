using Microsoft.UI.Xaml.Media;

namespace SpaceEngineersBlueprintEditor.Model;

public class BlueprintInfoViewData(ImageSource blueprintImage, bool noBlueprint, string name, string fileSize, string filePath)
{
    public ImageSource BlueprintImage { get; set; } = blueprintImage;
    public bool NoBlueprint { get; set; } = noBlueprint;
    public string Name { get; set; } = name;
    public string FileSize { get; set; } = fileSize;
    public string FilePath { get; set; } = filePath;
}
