using Microsoft.UI.Xaml.Hosting;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Utilities;
using System.Numerics;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

internal class NavigationService : GlobalServiceBase, ICustomNavigationService
{
    private Frame? frame;
    private readonly List<(Page, object?)> navigationStack = [];
    public bool CanGoBack => navigationStack.Count > 1;
    public bool CanGoForward => frame is not null && frame.CanGoForward;
    public Frame? Frame { get => frame; set => frame = value; }

    public List<(Page, object?)> NavigationStack => navigationStack;

    public event EventHandler<Type>? Navigated;

    public void GoBack() => NavigateTo(navigationStack[^2].Item1.GetType(), navigationStack[^2].Item2, true);

    public void GoForward() => frame?.GoForward();

    public void NavigateTo(Type type, object? parameter = null, bool goBack = false)
    {
        if (frame is not null && (navigationStack.Count == 0 || navigationStack.Last().Item1.GetType() != type || parameter is not null && navigationStack.Last().Item2 != parameter))
        {
            if (!PageManager.CurrentPages.TryGetValue(type.FullName!, out var currentPage))
            {
                var instance = Activator.CreateInstance(type);
                if (instance is Page pageInstance)
                {
                    currentPage = pageInstance;
                }
            }
            if (goBack)
            {
                navigationStack.RemoveAt(navigationStack.Count - 1);
            }
            else
            {
                currentPage?.SetParameter(parameter);
                navigationStack.Add((currentPage!, parameter));
            }
            var compositor = App.MainWindow.Compositor;
            var pageCompositor = ElementCompositionPreview.GetElementVisual(currentPage);
            pageCompositor.Offset = new Vector3(0, 100, 0);
            var slideUpAnimation = compositor.CreateVector3KeyFrameAnimation();
            slideUpAnimation.Target = "Offset";
            slideUpAnimation.InsertKeyFrame(0f, new Vector3(0, 100, 0));
            slideUpAnimation.InsertKeyFrame(1f, new Vector3(0, 0, 0), compositor.CreateCubicBezierEasingFunction(new Vector2(0.1f, 0.0f), new Vector2(0.0f, 1f)));
            slideUpAnimation.Duration = TimeSpan.FromSeconds(0.3);
            pageCompositor.StartAnimation("Offset", slideUpAnimation);
            frame.Content = currentPage;
            Navigated?.Invoke(frame, type);
        }
    }

    public void NavigateTo<T>(object? parameter = null) where T : Page => NavigateTo(typeof(T), parameter);

    public void NavigateTo(string pageName, object? parameter = null)
    {
        if (PageManager.PageDefinitions.TryGetValue(pageName, out var pageType))
            NavigateTo(pageType, parameter);
    }
}
