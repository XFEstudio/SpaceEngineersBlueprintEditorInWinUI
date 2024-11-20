using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;
using SpaceEngineersBlueprintEditor.ViewModels;
using System.Collections;
using XFEExtension.NetCore.XFETransform;

namespace SpaceEngineersBlueprintEditor.Model;

public partial class BlueprintPropertyViewData : ViewModelBase
{
    [ObservableProperty]
    private object? value;
    [ObservableProperty]
    private string? valueInString;
    public Type? Type { get; set; }
    public string? Name { get; set; }
    public string? CustomName { get; set; }
    public BlueprintPropertyViewData? Parent { get; set; }
    public ImageSource? CubeImage { get; set; }
    public List<BlueprintPropertyViewData> Children { get; set; } = [];
    public bool IsEnumerable => Type is not null && Type.IsAssignableTo(typeof(IEnumerable)) && Type != typeof(string);
    public bool IsBasicType => Type is not null && (XFEConverter.IsBasicType(Type) || Type.IsAssignableTo(typeof(Enum)));
    public bool IsNotBasicType => !IsBasicType;

    partial void OnValueChanged(object? value) => ValueInString = string.IsNullOrEmpty(Value?.ToString()) ? "值为空" : Value?.ToString();
}
