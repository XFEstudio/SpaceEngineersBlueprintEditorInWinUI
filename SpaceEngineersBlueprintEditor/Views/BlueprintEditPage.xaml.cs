using Microsoft.UI.Composition;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.ViewModels;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// À¶Í¼±à¼­Ò³Ãæ
/// </summary>
public sealed partial class BlueprintEditPage : Page
{
    private object? parameter;

    public object? Parameter
    {
        get { return parameter; }
        set { parameter = value; ViewModel.NavigationParameterService.OnParameterChange(parameter); }
    }

    public static BlueprintEditPage? Current { get; set; }
    public BlueprintEditPageViewModel ViewModel { get; set; } = new();
    private Compositor compositor = App.MainWindow.Compositor;
    public BlueprintEditPage()
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