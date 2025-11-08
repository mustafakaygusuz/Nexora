using Nexora.Core.Common.Configurations;
using Nexora.Core.Common.Helpers;

namespace Nexora.Core.Common.JsonConverters
{
    public sealed class EncryptIdNullableRewriteConverter(List<string> encryptedProperties, EncryptionConfigurationModel configuration) : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(long?) || objectType == typeof(List<long?>);
        }

        public override object? ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.TokenType == Newtonsoft.Json.JsonToken.StartArray)
            {
                var list = new List<long?>();

                while (reader.Read())
                {
                    if (reader.TokenType == Newtonsoft.Json.JsonToken.EndArray)
                    {
                        break;
                    }

                    list.Add(ReadSingleValue(reader));
                }
                return list;
            }

            return ReadSingleValue(reader);
        }

        private long? ReadSingleValue(Newtonsoft.Json.JsonReader reader)
        {
            if (reader.Value == null || string.IsNullOrEmpty(reader.Value.ToString()))
            {
                return null;
            }

            if (reader.Value is string stringValue && long.TryParse(stringValue, out var value))
            {
                return value;
            }

            if (long.TryParse(reader.Value.ToString(), out var number))
            {
                return number;
            }

            if (encryptedProperties.Contains(GetJsonPath(reader.Path)))
            {
                var encryptionConfig = configuration.EncryptionConfigurations?.FirstOrDefault(x => x.Name == "IdEncryption");

                return EncryptionHelper.DecryptNumeric(reader.Value.ToString(), encryptionConfig?.Key ?? "", encryptionConfig?.IV ?? "") ?? default;
            }
            else
            {
                return long.Parse(reader.Value.ToString());
            }
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value is List<long?> list)
            {
                writer.WriteStartArray();
                foreach (var item in list)
                {
                    WriteSingleValue(writer, item);
                }
                writer.WriteEndArray();
            }
            else
            {
                WriteSingleValue(writer, (long?)value);
            }
        }

        public void WriteSingleValue(Newtonsoft.Json.JsonWriter writer, long? value)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            if (encryptedProperties.Contains(GetJsonPath(writer.Path)))
            {
                var encryptionConfig = configuration?.EncryptionConfigurations?.FirstOrDefault(x => x.Name == "IdEncryption");

                writer.WriteValue(EncryptionHelper.EncryptNumeric(value.Value, encryptionConfig?.Key ?? "", encryptionConfig?.IV ?? ""));
            }
            else
            {
                writer.WriteValue(value.Value);
            }
        }

        private static string GetJsonPath(string path)
        {
            path = path.ToLower();

            while (path.Contains('['))
            {
                var openIndex = path.IndexOf('[');
                var closeIndex = path.IndexOf(']');

                path = path.Substring(0, openIndex) + path.Substring(closeIndex + 1);
            }

            return path;
        }
    }
}