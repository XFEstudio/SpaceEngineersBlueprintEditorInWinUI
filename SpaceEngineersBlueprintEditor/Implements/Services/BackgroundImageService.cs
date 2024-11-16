using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using SpaceEngineersBlueprintEditor.Interface.Services;
using System.Diagnostics.CodeAnalysis;
using Windows.UI;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

class BackgroundImageService : GlobalServiceBase, IBackgroundImageService
{
    private Page? _page;
    private Grid? _grid;

    [MemberNotNull(nameof(_page), nameof(_grid))]
    public void Initialize(Page page, Grid grid)
    {
        _page = page;
        _grid = grid;
    }

    public void ResetBackground()
    {
        if (_page is not null)
            _page.Background = new SolidColorBrush(Colors.Transparent);
        if (_grid is not null)
            _grid.Background = new SolidColorBrush(Colors.Transparent);
    }

    public void SetBackgroundImage(ImageSource imageSource)
    {
        if (_page is not null)
            _page.Background = new ImageBrush
            {
                ImageSource = imageSource
            };
        if (_grid is not null)
            _grid.Background = new AcrylicBrush
            {
                TintLuminosityOpacity = 0,
                TintColor = Colors.Transparent
            };
    }
}
