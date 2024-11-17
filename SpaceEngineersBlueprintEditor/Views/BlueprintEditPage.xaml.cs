using Microsoft.UI.Xaml.Navigation;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.ViewModels;
using VRage.Game;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// À¶Í¼±à¼­Ò³Ãæ
/// </summary>
public sealed partial class BlueprintEditPage : Page
{
    public MyObjectBuilder_Definitions? Parameter { get; set; }
    public static BlueprintEditPage? Current { get; set; }
    public BlueprintEditPageViewModel ViewModel { get; set; } = new();
    public BlueprintEditPage()
    {
        PageManager.AddOrUpdateCurrentPage(Current = this);
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (e.Parameter is MyObjectBuilder_Definitions parameter)
        {
            Parameter = parameter;
            ViewModel.NavigationParameterService.OnParameterChange(Parameter);
        }
    }
}