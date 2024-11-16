using Microsoft.UI.Xaml.Media;

namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface IBackgroundImageService : IGlobalService
{
    void Initialize(Page page, Grid grid);
    void SetBackgroundImage(ImageSource imageSource);
    void ResetBackground();
}
