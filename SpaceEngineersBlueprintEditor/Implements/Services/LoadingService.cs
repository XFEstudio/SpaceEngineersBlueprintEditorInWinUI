using SpaceEngineersBlueprintEditor.Interface.Services;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

internal class LoadingService : GlobalServiceBase, ILoadingService
{
    private Grid? _loadingGrid;
    private TextBlock? _textBlock;
    public void Initialize(Grid loadingGrid, TextBlock textBlock)
    {
        _loadingGrid = loadingGrid;
        _textBlock = textBlock;
    }

    public void StartLoading(string showText = "Loading...")
    {
        if (_loadingGrid is not null && _textBlock is not null)
        {
            _textBlock.Text = showText;
            _loadingGrid.Visibility = Visibility.Visible;
        }
    }

    public void StopLoading()
    {
        if (_loadingGrid is not null)
            _loadingGrid.Visibility = Visibility.Collapsed;
    }
}
