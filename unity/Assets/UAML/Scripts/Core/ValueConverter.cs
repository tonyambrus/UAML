using System;
using System.ComponentModel;
using System.Globalization;

namespace Uaml.Core
{
    public static class ValueConverter
    {
        public static bool TryConvert<T>(string value, out T result)
        {
            if (TryConvert(value, typeof(T), out var o))
            {
                result = (T)o;
                return true;
            }

            result = default;
            return false;
        }

        public static bool TryConvert(string value, Type targetType, out object result)
        {
            var converter = TypeDescriptor.GetConverter(targetType);
            try
            {
                result = converter.ConvertFromString(null, CultureInfo.InvariantCulture, value);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static object Convert(object value, Type targetType) => Convert(value, targetType, CultureInfo.InvariantCulture);
        public static object Convert(object value, Type targetType, CultureInfo culture)
        {
            var converter = TypeDescriptor.GetConverter(targetType);
            return converter.ConvertFrom(null, culture, value);
        }
    }
}