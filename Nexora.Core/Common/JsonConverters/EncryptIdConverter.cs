using Nexora.Core.Common.Attributes;
using Nexora.Core.Common.Configurations;
using Nexora.Core.Common.Helpers;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nexora.Core.Common.JsonConverters
{
    public sealed class EncryptIdConverter<T> : JsonConverter<T> where T : class
    {
        private readonly EncryptionConfigurationModel _configuration;
        private readonly EncryptionConfigurationModel.ConfigurationModel? _idEncryptionConfig;

        public EncryptIdConverter(EncryptionConfigurationModel configuration)
        {
            _configuration = configuration;
            _idEncryptionConfig = _configuration.EncryptionConfigurations?
                .FirstOrDefault(x => x.Name == "IdEncryption");
        }

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            var json = jsonDocument.RootElement.GetRawText();
            return JsonSerializer.Deserialize<T>(json, options);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (_idEncryptionConfig == null)
            {
                JsonSerializer.Serialize(writer, value, options);
                return;
            }

            var json = JsonSerializer.Serialize(value, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            using var document = JsonDocument.Parse(json);

            if (document.RootElement.ValueKind == JsonValueKind.Array)
            {
                Type itemType = typeof(T);
                if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                {
                    itemType = typeof(T).GetGenericArguments()[0];
                }

                var encryptedProps = GetEncryptedProperties(itemType);
                var processedArray = new List<JsonElement>();

                foreach (var item in document.RootElement.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.Object)
                    {
                        var itemDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(item.GetRawText());

                        if (itemDict != null)
                        {
                            foreach (var propPath in encryptedProps)
                            {
                                EncryptPropertyInDict(itemDict, propPath.Split('.'));
                            }

                            processedArray.Add(JsonDocument.Parse(JsonSerializer.Serialize(itemDict)).RootElement);
                        }
                    }
                    else
                    {
                        processedArray.Add(item);
                    }
                }

                var finalJson = JsonSerializer.Serialize(processedArray, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                JsonSerializer.Serialize(writer, processedArray, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            else if (document.RootElement.ValueKind == JsonValueKind.Object)
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

                if (dict == null)
                {
                    JsonSerializer.Serialize(writer, value, options);
                    return;
                }

                var encryptedProps = GetEncryptedProperties(typeof(T));
                foreach (var propPath in encryptedProps)
                {
                    EncryptPropertyInDict(dict, propPath.Split('.'));
                }

                var finalJson = JsonSerializer.Serialize(dict, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                JsonSerializer.Serialize(writer, dict, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            else
            {
                JsonSerializer.Serialize(writer, value, options);
            }
        }

        private void EncryptPropertyInDict(Dictionary<string, JsonElement> dict, string[] path)
        {
            if (path.Length == 0) return;

            var key = path[0];
            var camelKey = char.ToLowerInvariant(key[0]) + key.Substring(1);

            if (path.Length == 1)
            {
                if (dict.TryGetValue(camelKey, out var element))
                {
                    if (element.ValueKind == JsonValueKind.Number && element.TryGetInt64(out var id))
                    {
                        var encrypted = EncryptionHelper.EncryptNumeric(
                            id,
                            _idEncryptionConfig!.Key ?? "",
                            _idEncryptionConfig.IV ?? ""
                        );
                        dict[camelKey] = JsonDocument.Parse($"\"{encrypted}\"").RootElement;
                    }
                }
            }
            else
            {
                if (dict.TryGetValue(camelKey, out var element) &&
                    element.ValueKind == JsonValueKind.Object)
                {
                    var nestedDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
                        element.GetRawText()
                    );

                    if (nestedDict != null)
                    {
                        EncryptPropertyInDict(nestedDict, path.Skip(1).ToArray());
                        dict[camelKey] = JsonDocument.Parse(
                            JsonSerializer.Serialize(nestedDict)
                        ).RootElement;
                    }
                }
                else if (element.ValueKind == JsonValueKind.Array)
                {
                    var array = element.EnumerateArray()
                        .Select(item =>
                        {
                            if (item.ValueKind == JsonValueKind.Object)
                            {
                                var itemDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
                                    item.GetRawText()
                                );
                                if (itemDict != null)
                                {
                                    EncryptPropertyInDict(itemDict, path.Skip(1).ToArray());
                                    return JsonDocument.Parse(
                                        JsonSerializer.Serialize(itemDict)
                                    ).RootElement;
                                }
                            }
                            return item;
                        })
                        .ToList();

                    dict[camelKey] = JsonDocument.Parse(
                        JsonSerializer.Serialize(array)
                    ).RootElement;
                }
            }
        }

        private List<string> GetEncryptedProperties(Type type, string prefix = "")
        {
            var result = new List<string>();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var hasAttribute = prop.GetCustomAttribute<EncryptedIdAttribute>() != null;
                var currentPath = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";

                if (hasAttribute)
                {
                    result.Add(currentPath);
                }
                else if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                {
                    if (prop.PropertyType.IsGenericType &&
                        prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        var itemType = prop.PropertyType.GetGenericArguments()[0];
                        if (itemType.IsClass && itemType != typeof(string))
                        {
                            result.AddRange(GetEncryptedProperties(itemType, currentPath));
                        }
                    }
                    else
                    {
                        result.AddRange(GetEncryptedProperties(prop.PropertyType, currentPath));
                    }
                }
            }

            return result;
        }
    }
}