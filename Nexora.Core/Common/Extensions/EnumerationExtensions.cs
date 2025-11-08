using Nexora.Core.Common.Attributes;
using Nexora.Core.Common.Models;
using System.ComponentModel;
using System.Reflection;

namespace Nexora.Core.Common.Extensions
{
    public static class EnumerationExtensions
    {
        public static List<DropdownDataModel> GetDropdownModel<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                    .Cast<T>()
                    .Where(e => !e.GetType()
                                 .GetField(e.ToString())
                                 .GetCustomAttributes<ExcludeEnum>()
                                 .Any())
                    .Select(e => new DropdownDataModel
                    {
                        Value = Convert.ToInt32(e),
                        Key = e.ToString(),
                        Description = e.GetDescription(),
                    })
                    .ToList();
        }

        public static string GetDescription<T>(this T value) where T : Enum
            => value.GetType()
                .GetField(value.ToString()!)?
                .GetCustomAttribute<DescriptionAttribute>()?
                .Description ?? string.Empty;

        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T ParseEnum<T>(this byte value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        public static T ParseEnumSafe<T>(this string value, T defaultValue = default) where T : struct
        {
            if (Enum.TryParse<T>(value, true, out var result))
                return result;
            return defaultValue;
        }
    }
}