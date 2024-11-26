using Microsoft.UI.Xaml.Media;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Utilities;
using System.Collections;
using System.Reflection;
using VRage.Game;
using XFEExtension.NetCore.XFETransform;

namespace SpaceEngineersBlueprintEditor.Model;

/// <summary>
/// 蓝图属性视图数据
/// </summary>
public partial class BlueprintPropertyViewData
{
    /// <summary>
    /// 属性值
    /// </summary>
    public object? Value { get; set; }
    /// <summary>
    /// 属性值的字符串形式
    /// </summary>
    public string? ValueString => Value?.ToString();
    /// <summary>
    /// 自定义数据
    /// </summary>
    public object? CustomData { get; set; }
    /// <summary>
    /// 属性类型
    /// </summary>
    public Type? Type { get; set; }
    /// <summary>
    /// 属性名称
    /// </summary>
    public string? Name { get; set; }
    public string NameTypeString => $"{Name}[{Type?.Name}]";
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
    /// 字符串控件
    /// </summary>
    public TextBox StringControl
    {
        get
        {
            var textBox = new TextBox
            {
                Text = ValueString
            };
            textBox.TextChanged += (sender, e) =>
            {
                if (sender is TextBox currentTextBox && Type is not null)
                    SetValue(Convert.ChangeType(currentTextBox.Text, Type));
            };
            return textBox;
        }
    }
    /// <summary>
    /// 数字控件
    /// </summary>
    public NumberBox NumberControl
    {
        get
        {
            var numberBox = new NumberBox
            {
                SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Compact,
                Value = ValueString is not null ? double.Parse(ValueString) : 0
            };
            numberBox.ValueChanged += (sender, args) =>
            {
                if (Type is not null)
                    SetValue(Convert.ChangeType(sender.Value, Type));
            };
            return numberBox;
        }
    }
    /// <summary>
    /// 枚举控件
    /// </summary>
    public ComboBox EnumControl
    {
        get
        {
            var comboBox = new ComboBox
            {
                ItemsSource = EnumValues,
                SelectedItem = ValueString
            };
            comboBox.SelectionChanged += (sender, e) =>
            {
                if (e.AddedItems.FirstOrDefault() is string value && Type is not null)
                    SetValue(Enum.Parse(Type, value));
            };
            return comboBox;
        }
    }
    /// <summary>
    /// 复合枚举控件
    /// </summary>
    public SplitButton MultiEnumControl
    {
        get
        {
            var stackPanel = new StackPanel();
            foreach (var enumItem in EnumValues)
            {
                stackPanel.Children.Add(new CheckBox
                {
                    Content = enumItem,
                    IsChecked = ValueString?.Contains(enumItem)
                });
            }
            var splitButton = new SplitButton
            {
                Content = ValueString,
                Flyout = new Flyout
                {
                    Content = stackPanel
                }
            };
            splitButton.Flyout.Closed += (sender, e) =>
            {
                if (sender is Flyout flyout && Type is not null)
                {
                    var targetValue = string.Join(", ", stackPanel.Children.Where(child => child is CheckBox checkBox && checkBox.IsChecked is not null && checkBox.IsChecked.Value).Cast<CheckBox>().Select(checkBox => checkBox.Content.ToString()));
                    SetValue(Enum.Parse(Type, targetValue));
                    splitButton.Content = targetValue;
                }
            };
            return splitButton;
        }
    }
    /// <summary>
    /// 布尔值控件
    /// </summary>
    public ComboBox BoolControl
    {
        get
        {
            var comboBox = new ComboBox
            {
                ItemsSource = EnumValues,
                SelectedItem = Value?.ToString()
            };
            comboBox.SelectionChanged += (sender, e) =>
            {
                if (e.AddedItems.FirstOrDefault() is string value && Type is not null)
                    SetValue(Convert.ChangeType(value, Type));
            };
            return comboBox;
        }
    }
    /// <summary>
    /// 自动选择最合适的控件
    /// </summary>
    public object? BestControl
    {
        get
        {
            if (IsBasicType)
            {
                if (IsMultiEnum)
                    return MultiEnumControl;
                else if (Type is not null && Type.IsEnum)
                    return EnumControl;
                else if (Type == typeof(int) || Type == typeof(double) || Type == typeof(float) || Type == typeof(short) || Type == typeof(byte) || Type == typeof(uint) || Type == typeof(ushort))
                    return NumberControl;
                else if (Type == typeof(bool))
                    return BoolControl;
                else
                    return StringControl;
            }
            else
            {
                return null;
            }
        }
    }
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

    /// <summary>
    /// 设置值
    /// </summary>
    /// <param name="targetValue">目标值</param>
    public void SetValue(object? targetValue)
    {
        try
        {
            if (Parent is BlueprintPropertyViewData parentBlueprintPropertyViewData && Name is not null && parentBlueprintPropertyViewData.Type is not null)
            {
                var memberInfo = parentBlueprintPropertyViewData.Type.GetMember(Name).Where(memberInfo => memberInfo is FieldInfo || memberInfo is PropertyInfo).FirstOrDefault();
                if (memberInfo is FieldInfo fieldInfo)
                {
                    fieldInfo.SetValue(Parent.Value, targetValue);
                }
                else if (memberInfo is PropertyInfo propertyInfo)
                {
                    propertyInfo.SetValue(Parent.Value, targetValue);
                }
                Value = targetValue;
                if (parentBlueprintPropertyViewData.Type.IsValueType)
                    SetValueType(parentBlueprintPropertyViewData);
            }
        }
        catch (Exception ex)
        {
            GlobalServiceManager.GetService<IMessageService>()?.ShowMessage($"{"Error_CantSetValue_CantSetProperty".GetLocalized()}{Name}({Type?.Name}){"Error_CantSetValue_ValueTo".GetLocalized()}{targetValue}: {ex.Message}", "Error".GetLocalized(), InfoBarSeverity.Error);
        }
    }

    private static void SetValueType(BlueprintPropertyViewData blueprintPropertyViewData)
    {
        if (blueprintPropertyViewData.Parent is BlueprintPropertyViewData parentBlueprintViewData)
        {
            if (blueprintPropertyViewData.Type is not null && blueprintPropertyViewData.Type.IsValueType)
                blueprintPropertyViewData.SetValue(blueprintPropertyViewData.Value);
            SetValueType(parentBlueprintViewData);
        }
    }
}
