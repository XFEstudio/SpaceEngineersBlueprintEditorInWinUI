using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;
using SpaceEngineersBlueprintEditor.ViewModels;
using System.Collections;
using System.Reflection;
using XFEExtension.NetCore.XFETransform;

namespace SpaceEngineersBlueprintEditor.Model;

/// <summary>
/// 蓝图属性视图数据
/// </summary>
public partial class BlueprintPropertyViewData : ViewModelBase
{
    /// <summary>
    /// 属性值
    /// </summary>
    [ObservableProperty]
    private object? value;
    /// <summary>
    /// 属性值的字符串形式
    /// </summary>
    [ObservableProperty]
    private string? valueInString;
    /// <summary>
    /// 自定义数据
    /// </summary>
    [ObservableProperty]
    private object? customData;
    /// <summary>
    /// 属性类型
    /// </summary>
    public Type? Type { get; set; }
    /// <summary>
    /// 属性名称
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// 属性枚举值
    /// </summary>
    public string[] EnumValues => Type is not null ? Type.IsEnum ? Enum.GetNames(Type) : ["True", "False"] : [];
    /// <summary>
    /// 属性的父属性
    /// </summary>
    public BlueprintPropertyViewData? Parent { get; set; }
    /// <summary>
    /// 方块图片
    /// </summary>
    public ImageSource? CubeImage { get; set; }
    /// <summary>
    /// 属性的子属性
    /// </summary>
    public List<BlueprintPropertyViewData> Children { get; set; } = [];
    /// <summary>
    /// 是否是枚举类型
    /// </summary>
    public bool IsEnumerable => Type is not null && Type.IsAssignableTo(typeof(IEnumerable)) && Type != typeof(string);
    /// <summary>
    /// 是否是复合枚举类型
    /// </summary>
    public bool IsMultiEnum => Type is not null && Type.IsDefined(typeof(FlagsAttribute), false);
    /// <summary>
    /// 是否是基本类型
    /// </summary>
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
