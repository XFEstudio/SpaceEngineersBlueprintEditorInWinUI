using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;
using SpaceEngineersBlueprintEditor.ViewModels;
using System.Collections;
using System.Reflection;
using XFEExtension.NetCore.XFETransform;

namespace SpaceEngineersBlueprintEditor.Model;

public partial class BlueprintPropertyViewData : ViewModelBase
{
    [ObservableProperty]
    private object? value;
    [ObservableProperty]
    private string? valueInString;
    [ObservableProperty]
    private object? customData;
    public Type? Type { get; set; }
    public string? Name { get; set; }
    public string[] EnumValues => Type is not null ? Type.IsEnum ? Enum.GetNames(Type) : ["True", "False"] : [];
    public BlueprintPropertyViewData? Parent { get; set; }
    public ImageSource? CubeImage { get; set; }
    public List<BlueprintPropertyViewData> Children { get; set; } = [];
    public bool IsEnumerable => Type is not null && Type.IsAssignableTo(typeof(IEnumerable)) && Type != typeof(string);
    public bool IsMultiEnum => Type is not null && Type.IsDefined(typeof(FlagsAttribute), false);
    public bool IsBasicType => Type is not null && (XFEConverter.IsBasicType(Type) || Type.IsEnum);

    partial void OnValueChanged(object? value)
    {
        try
        {
            ValueInString = Value?.ToString();
            if (Type is null || value is null)
                return;
            if (Type.IsEnum && ValueInString is not null)
                SetValue(this, Enum.Parse(Type, ValueInString));
            else
                SetValue(this, Convert.ChangeType(value, Type));
        }
        catch { }
    }

    private static void SetValue(BlueprintPropertyViewData blueprintPropertyViewData, object? targetValue)
    {
        if (blueprintPropertyViewData.Parent is not null && blueprintPropertyViewData.Name is not null && blueprintPropertyViewData.Parent.Type is not null)
        {
            var memberInfoList = blueprintPropertyViewData.Parent.Type.GetMember(blueprintPropertyViewData.Name);
            foreach (var memberInfo in memberInfoList)
            {
                if (memberInfo is FieldInfo fieldInfo)
                {
                    fieldInfo.SetValue(blueprintPropertyViewData.Parent.Value, targetValue);
                }
                else if (memberInfo is PropertyInfo propertyInfo)
                {
                    propertyInfo.SetValue(blueprintPropertyViewData.Parent.Value, targetValue);
                }
            }
        }
    }
}
