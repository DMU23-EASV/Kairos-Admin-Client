using System.Globalization;
using System.Windows.Data;

namespace WPF_MVVM_TEMPLATE.Application.Utility;

public class InverseBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool && !(bool)value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool && !(bool)value;
    }
}