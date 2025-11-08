using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nexora.Core.Common.JsonConverters
{
    public sealed class SensitiveDataField : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(new string('*', value.Length));
        }
    }
}