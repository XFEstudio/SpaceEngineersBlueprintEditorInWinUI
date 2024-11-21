using Microsoft.UI.Xaml.Data;

namespace SpaceEngineersBlueprintEditor.Utilities.Converter;

public partial class BaseTypeConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null)
            return null;
        else if (value.GetType().IsEnum)
            return value.ToString();
        else if (value.GetType() == typeof(bool))
            return value.ToString();
        return System.Convert.ChangeType(value?.ToString(), targetType);
    }

    public object? ConvertBack(object value, Type targetType, object parameter, string language) => value;
}
