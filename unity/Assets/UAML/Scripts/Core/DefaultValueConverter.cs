using System;
using System.Globalization;

namespace Uaml.Core
{
    public class DefaultValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ValueConverter.Convert(value, targetType, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ValueConverter.Convert(value, targetType, culture);
        }
    }
}