using Microsoft.UI.Composition;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.ViewModels;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// Ö÷Ò³Ãæ
/// </summary>
public sealed partial class MainPage : Page
{
    public static MainPage? Current { get; set; }
    public MainPageViewModel ViewModel { get; set; } = new();
    private Compositor compositor = App.MainWindow.Compositor;
    public MainPage()
    {
        PageManager.AddOrUpdateCurrentPage(Current = this);
        this.InitializeComponent();
        ViewModel.FileDropService.Initialize(blueprintDragGrid, compositor);
        var expressionAnimation = compositor.CreateExpressionAnimation();
        expressionAnimation.Expression = "(dragElement.Scale.Y - 1) * 200";
        expressionAnimation.Target = "Translation.Y";
        expressionAnimation.SetExpressionReferenceParameter("dragElement", blueprintDragGrid);
        viewBlueprintsList.StartAnimation(expressionAnimation);
        openLocalFile.StartAnimation(expressionAnimation);
    }
}
