namespace SpaceEngineersBlueprintEditor.Model;

public class BlueprintInfo(string imagePath, bool hasBlueprint, bool hasImage, string name, string fileSize, string blueprintPath)
{
    public string ImagePath { get; set; } = imagePath;
    public bool HasBlueprint { get; set; } = hasBlueprint;
    public bool HasImage { get; set; } = hasImage;
    public string Name { get; set; } = name;
    public string FileSize { get; set; } = fileSize;
    public string BlueprintPath { get; set; } = blueprintPath;
}
