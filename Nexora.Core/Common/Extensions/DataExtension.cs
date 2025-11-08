using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace Nexora.Core.Common.Extensions
{
    public static class DataExtension
    {
        public static bool HasValue<T>(this T data)
        {
            var t = typeof(T);

            if (data == null)
            {
                return false;
            }

            if (data is string str && string.IsNullOrEmpty(str))
            {
                return false;
            }

            if (t.IsGenericType && data is IList list && list.Count == 0 || data is Array array && array.Length == 0)
            {
                return false;
            }

            if (typeof(T).IsValueType && data.Equals(Activator.CreateInstance(typeof(T))))
            {
                return false;
            }

            if (data is IDictionary dict && dict.Count == 0)
            {
                return false;
            }

            return true;
        }

        public static T? ToConvert<T>(this object value)
        {
            var conversionType = typeof(T);

            if (value == null)
            {
                return default;
            }

            if (conversionType == typeof(Guid))
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(value.ToString());
            }
            else
            {
                if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    conversionType = Nullable.GetUnderlyingType(conversionType);
                }

                return (T)Convert.ChangeType(value, conversionType);
            }
        }

        public static object? ToConvert(this object value, Type conversionType)
        {
            if (value == null)
            {
                return default;
            }

            if (conversionType == typeof(Guid))
            {
                return new Guid(value.ToString() ?? "");
            }
            else
            {
                if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    conversionType = Nullable.GetUnderlyingType(conversionType);
                }

                return Convert.ChangeType(value, conversionType);
            }
        }

        public static int ToInt<T>(this T data)
        {
            return Convert.ToInt32(data);
        }

        public static byte ToByte<T>(this T data)
        {
            return Convert.ToByte(data);
        }

        public static short ToShort<T>(this T data)
        {
            return Convert.ToInt16(data);
        }

        public static double ToDouble<T>(this T data)
        {
            return Convert.ToDouble(data);
        }

        public static decimal ToDecimal<T>(this T data)
        {
            return Convert.ToDecimal(data);
        }

        public static bool ToBoolean(this string data)
        {
            bool.TryParse(data, out var result);

            return result;
        }

        public static TimeSpan? ToTimeSpan(this string data)
        {
            if (data.Length < 6)
                return TimeSpan.TryParseExact(data, "hh\\:mm", CultureInfo.CurrentCulture, out var result) ? TimeSpan.Parse(data) : null;
            else
                return TimeSpan.TryParseExact(data, "hh\\:mm\\:ss", CultureInfo.CurrentCulture, out var result) ? TimeSpan.Parse(data) : null;
        }

        public static T ToObject<T>(this IDictionary<string, string> data) where T : class, new()
        {
            var obj = new T();
            var objType = obj.GetType();

            foreach (var item in data)
            {
                var property = objType.GetProperty(item.Key);

                if (property != null)
                {
                    object value;

                    var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                    try
                    {
                        if (propertyType == typeof(Guid))
                        {
                            value = Guid.Parse(item.Value);
                        }
                        else if (propertyType.IsEnum)
                        {
                            value = Enum.Parse(propertyType, item.Value);
                        }
                        else
                        {
                            value = Convert.ChangeType(item.Value, propertyType);
                        }
                    }
                    catch
                    {
                        value = null;
                    }

                    property.SetValue(obj, value, null);
                }
            }

            return obj;
        }

        public static IDictionary<string, object> ToDictionary(this object source)
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var property in source.GetType().GetProperties())
            {
                var value = property.GetValue(source, null);

                if (value.HasValue())
                    dictionary.Add(property.Name, value!);
            }

            return dictionary;
        }

        public static long ToLong<T>(this T data)
        {
            return Convert.ToInt64(data);
        }

        public static bool Equal(this string str1, string str2, bool ignoreCase = true)
        {
            return string.Equals(str1, str2, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }

        public static bool Equal<T>(this List<T>? list1, List<T>? list2)
        {
            if (!list1.HasValue() || !list2.HasValue())
            {
                return list1 == list2;
            }

            if (list1!.Count != list2!.Count)
            {
                return false;
            }

            return list1.OrderBy(x => x).SequenceEqual(list2.OrderBy(x => x));
        }
    }
}