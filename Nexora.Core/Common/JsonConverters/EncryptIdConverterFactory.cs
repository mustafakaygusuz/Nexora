using Nexora.Core.Common.Attributes;
using Nexora.Core.Common.Configurations;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nexora.Core.Common.JsonConverters
{
    public sealed class EncryptIdConverterFactory : JsonConverterFactory
    {
        private readonly EncryptionConfigurationModel _configuration;

        public EncryptIdConverterFactory(EncryptionConfigurationModel configuration)
        {
            _configuration = configuration;
        }

        public override bool CanConvert(Type typeToConvert)
        {
            var properties = typeToConvert.GetProperties();
            return properties != null && properties.Any(IsEncryptIdProperty);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return (JsonConverter)Activator.CreateInstance(typeof(EncryptIdConverter<>).MakeGenericType(typeToConvert), _configuration);
        }

        private static bool IsEncryptIdProperty(PropertyInfo property)
        {
            return property.GetCustomAttribute<EncryptedIdAttribute>() != null;
        }
    }
}