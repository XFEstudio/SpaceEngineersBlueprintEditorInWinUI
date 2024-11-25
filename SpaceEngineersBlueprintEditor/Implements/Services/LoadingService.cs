using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media;
using SpaceEngineersBlueprintEditor.Interface.Services;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

///<inheritdoc cref="ILoadingService"/>
internal class LoadingService : GlobalServiceBase, ILoadingService
{
    private Grid? _loadingGrid;
    private DispatcherQueue? _dispatcherQueue;
    private IAutoNavigationService? _navigationService;
    private readonly Dictionary<Type, Grid> pageGridDictionary = [];

    public IAutoNavigationService? NavigationService => _navigationService;

    public void Initialize(Grid loadingGrid, DispatcherQueue dispatcherQueue, IAutoNavigationService? navigationService)
    {
        _loadingGrid = loadingGrid;
        _dispatcherQueue = dispatcherQueue;
        _navigationService = navigationService;
        _navigationService!.Navigated += NavigationService_Navigated;
    }

    private void NavigationService_Navigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        pageGridDictionary.Values.ForEach(grid => grid.Visibility = Visibility.Collapsed);
        if (pageGridDictionary.TryGetValue(e.SourcePageType, out var currentGrid))
            _dispatcherQueue?.TryEnqueue(() => currentGrid.Visibility = Visibility.Visible);
    }

    public bool StartLoading<T>(string showText = "Loading...") where T : Page
    {
        if (pageGridDictionary.TryGetValue(typeof(T), out var grid) && grid.FindName("loadingTextBlock") is TextBlock textBlock)
        {
            _dispatcherQueue?.TryEnqueue(() => textBlock.Text = showText);
            return true;
        }
        else
        {
            _dispatcherQueue?.TryEnqueue(() =>
            {
                var newGrid = CreateLoadingGrid(showText);
                pageGridDictionary.Add(typeof(T), newGrid);
                _loadingGrid?.Children.Add(newGrid);
            });
            return false;
        }
    }

    public bool StopLoading<T>() where T : Page
    {
        if (pageGridDictionary.TryGetValue(typeof(T), out var grid) && _loadingGrid is not null)
            return _dispatcherQueue?.TryEnqueue(() =>
            {
                _loadingGrid.Children.Remove(grid);
                pageGridDictionary.Remove(typeof(T));
            }) ?? false;
        return false;
    }

    private static Grid CreateLoadingGrid(string loadingText)
    {
        var loadingStackPanel = new StackPanel
        {
            Spacing = 30
        };
        loadingStackPanel.Children.Add(new TextBlock
        {
            Foreground = new SolidColorBrush(Colors.White),
            FontSize = 34,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Name = "loadingTextBlock",
            Text = loadingText
        });
        loadingStackPanel.Children.Add(new ProgressRing
        {
            IsActive = true,
            Width = 60,
            Height = 60,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        });
        var loadingGrid = new Grid
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            CornerRadius = new(15),
            Padding = new(100),
            Background = new SolidColorBrush(Colors.Black)
            {
                Opacity = 0.5
            }
        };
        loadingGrid.Children.Add(loadingStackPanel);
        var containerGrid = new Grid
        {
            Margin = new(0, -72, 0, 0),
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Background = (Brush)Application.Current.Resources["SmokeFillColorDefaultBrush"]
        };
        containerGrid.Children.Add(loadingGrid);
        return containerGrid;
    }
}
