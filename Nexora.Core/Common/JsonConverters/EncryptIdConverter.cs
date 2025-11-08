using Nexora.Core.Common.Attributes;
using Nexora.Core.Common.Configurations;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nexora.Core.Common.JsonConverters
{
    public sealed class EncryptIdConverter<T> : JsonConverter<T>
    {
        private readonly EncryptionConfigurationModel _configuration;

        public EncryptIdConverter(EncryptionConfigurationModel configuration)
        {
            _configuration = configuration;
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                using var jsonDocument = JsonDocument.ParseValue(ref reader);
                var jsonObject = jsonDocument.RootElement;

                var encyrptedProperties = FindPropertiesWithEncryptId(typeToConvert).Select(x => x.ToLower()).ToList();

                var settings = new Newtonsoft.Json.JsonSerializerSettings();
                settings.Converters.Add(new EncryptIdRewriteConverter(encyrptedProperties, _configuration));
                settings.Converters.Add(new EncryptIdNullableRewriteConverter(encyrptedProperties, _configuration));

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonObject.GetRawText(), settings);
            }
            catch (Exception)
            {
                throw new Exception($"JsonConverter read error {typeToConvert.FullName}");
            }
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            try
            {
                var encyrptedProperties = FindPropertiesWithEncryptId(typeof(T)).Select(x => x.ToLower()).ToList();

                var settings = new Newtonsoft.Json.JsonSerializerSettings();
                settings.Converters.Add(new EncryptIdRewriteConverter(encyrptedProperties, _configuration));
                settings.Converters.Add(new EncryptIdNullableRewriteConverter(encyrptedProperties, _configuration));
                settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                settings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

                var obj = Newtonsoft.Json.JsonConvert.SerializeObject(value, settings);

                using (JsonDocument document = JsonDocument.Parse(obj))
                {
                    document.RootElement.WriteTo(writer);
                }
            }
            catch (Exception)
            {
                throw new Exception($"JsonConverter write error {typeof(T).FullName}");
            }
        }

        private List<string> FindPropertiesWithEncryptId(Type type, List<string>? encyrptedProperties = null, string parent = "")
        {
            PropertyInfo[] properties = type.GetProperties();

            if (encyrptedProperties == null)
                encyrptedProperties = new List<string>();

            foreach (PropertyInfo property in properties.Where(x => x.GetCustomAttribute<EncryptedIdAttribute>() != null || (x.PropertyType.IsClass && x.PropertyType != typeof(string))))
            {
                if (property.PropertyType.IsGenericType && typeof(List<>).IsAssignableFrom(property.PropertyType.GetGenericTypeDefinition()))
                {
                    Type genericArgumentType = property.PropertyType.GetGenericArguments()[0];
                    if (genericArgumentType.IsClass)
                        FindPropertiesWithEncryptId(genericArgumentType, encyrptedProperties, parent + property.Name + ".");
                    else
                        encyrptedProperties.Add(parent + property.Name);
                }
                else if (property.PropertyType.IsClass)
                    FindPropertiesWithEncryptId(property.PropertyType, encyrptedProperties, parent + property.Name + ".");
                else
                    encyrptedProperties.Add(parent + property.Name);
            }

            return encyrptedProperties;
        }
    }
}