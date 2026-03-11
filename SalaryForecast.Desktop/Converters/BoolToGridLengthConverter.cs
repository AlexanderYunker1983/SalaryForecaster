using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace SalaryForecast.Desktop.Converters
{
    public class BoolToGridLengthConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var isVisible = value is true;
            return parameter?.ToString() == "separator"
                ? (isVisible ? new GridLength(16) : new GridLength(0))
                : (isVisible ? new GridLength(1, GridUnitType.Star) : new GridLength(0));
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
