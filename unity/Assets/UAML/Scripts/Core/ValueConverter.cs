using System;
using System.ComponentModel;
using System.Globalization;

namespace Uaml.Core
{
    public static class ValueConverter
    {
        public static bool TryParse<T>(string inValue, out T result)
        {
            if (TryParse(inValue, typeof(T), out var o))
            {
                result = (T)o;
                return true;
            }

            result = default;
            return false;
        }

        public static bool TryParse(string inValue, Type t, out object result)
        {
            var converter = TypeDescriptor.GetConverter(t);
            try
            {
                result = converter.ConvertFromString(null, CultureInfo.InvariantCulture, inValue);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
}