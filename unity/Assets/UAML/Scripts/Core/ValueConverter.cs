using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;
using UnityEngine;

namespace Uaml.Core
{
    public static class ValueConverter
    {
        //public static bool TryConvert<T>(string value, out T result)
        //{
        //    if (TryConvert(value, typeof(T), out var o))
        //    {
        //        result = (T)o;
        //        return true;
        //    }

        //    result = default;
        //    return false;
        //}

        //public static bool TryConvert(string value, Type targetType, out object result)
        //{
        //    try
        //    {
        //        var converter = TypeDescriptor.GetConverter(targetType);
        //        result = converter.ConvertFromString(null, CultureInfo.InvariantCulture, value);
        //        return true;
        //    }
        //    catch
        //    {
        //        result = JsonUtility.FromJson(value, targetType);
        //    }

        //    result = null;
        //    return false;
        //}

        public static object Convert(object value, Type targetType) => Convert(value, targetType, CultureInfo.InvariantCulture);
        public static object Convert(object value, Type targetType, CultureInfo culture)
        {
            if (targetType == typeof(string))
            {
                if (value is Color c)
                {
                    return ColorUtility.ToHtmlStringRGBA(c);
                }

                if (value is string)
                {
                    return value;
                }

                return JsonConvert.SerializeObject(value);
            }
            else if (value is string)
            {
                if (targetType == typeof(Color))
                {
                    return ColorUtility.TryParseHtmlString((string)value, out var c) ? c : default;
                }

                return JsonConvert.DeserializeObject((string)value, targetType);
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(targetType);
                return converter.ConvertFrom(value);
            }
        }
    }
}