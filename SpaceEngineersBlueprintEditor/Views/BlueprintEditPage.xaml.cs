using Microsoft.UI.Xaml.Navigation;
using SpaceEngineersBlueprintEditor.Model;
using SpaceEngineersBlueprintEditor.Utilities;
using SpaceEngineersBlueprintEditor.ViewModels;
using XFEExtension.NetCore.StringExtension;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// À¶Í¼±à¼­Ò³Ãæ
/// </summary>
public sealed partial class BlueprintEditPage : Page
{
    public BlueprintModel? Parameter { get; set; }
    public static BlueprintEditPage? Current { get; set; }
    public BlueprintEditPageViewModel ViewModel { get; set; } = new();
    public BlueprintEditPage()
    {
        PageManager.AddOrUpdateCurrentPage(Current = this);
        this.InitializeComponent();
        ViewModel.DialogService.RegisterDialog(editValueContentDialog);
        ViewModel.DialogService.RegisterDialog(editEnumContentDialog);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (e.Parameter is BlueprintModel parameter)
        {
            Parameter = parameter;
            if (e.SourcePageType != typeof(BlueprintDetailPage))
            {
                if (parameter.ViewData is not null)
                    ViewModel.BackgroundImageService?.SetBackgroundImage(parameter.ViewData.BlueprintImage);
            }
            ViewModel.NavigationParameterService.OnParameterChange(Parameter);
        }
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        if (e.SourcePageType != typeof(BlueprintDetailPage))
            ViewModel.BackgroundImageService?.ResetBackground();
    }

    private static void GetTreeNodes(TreeViewNode treeViewNode)
    {
        if (treeViewNode.Content is BlueprintPropertyViewData blueprintPropertyViewData && treeViewNode.HasUnrealizedChildren)
        {
            SpaceEngineersHelper.AnalyzeBlueprint(blueprintPropertyViewData);
            if (blueprintPropertyViewData.Children.Count == 0)
                return;
            foreach (var child in blueprintPropertyViewData.Children)
            {
                treeViewNode.Children.Add(new TreeViewNode
                {
                    Content = child,
                    HasUnrealizedChildren = !child.IsBasicType && child.Type is not null
                });
            }
            treeViewNode.HasUnrealizedChildren = false;
        }
    }

    private void TreeView_Expanding(TreeView sender, TreeViewExpandingEventArgs args) => GetTreeNodes(args.Node);

    private void TreeView_Collapsed(TreeView sender, TreeViewCollapsedEventArgs args)
    {
        if (args.Node.Content is BlueprintPropertyViewData blueprintPropertyViewData)
            blueprintPropertyViewData.Children.Clear();
        args.Node.Children.Clear();
        args.Node.HasUnrealizedChildren = true;
    }

    private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        if (sender.DataContext is BlueprintPropertyViewData blueprintPropertyViewData)
            blueprintPropertyViewData.Value = sender.Value;
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox && comboBox.DataContext is BlueprintPropertyViewData blueprintPropertyViewData && comboBox.SelectedItem?.ToString() is string value && !value.IsNullOrEmpty())
            blueprintPropertyViewData.Value = comboBox.SelectedItem;
    }

    private void Flyout_Closed(object sender, object e)
    {
        if (sender is Flyout flyout && flyout.Content is StackPanel stackPanel && stackPanel.DataContext is BlueprintPropertyViewData blueprintPropertyViewData && blueprintPropertyViewData.Type is not null)
            blueprintPropertyViewData.Value = Enum.Parse(blueprintPropertyViewData.Type, string.Join(", ", stackPanel.Children.Where(child => child is CheckBox checkBox && checkBox.IsChecked is not null && checkBox.IsChecked.Value).Cast<CheckBox>().Select(checkBox => checkBox.Content.ToString())));
    }
}