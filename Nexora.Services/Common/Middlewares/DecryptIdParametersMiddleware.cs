using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Nexora.Core.Common.Attributes;
using Nexora.Core.Common.Configurations;
using Nexora.Core.Common.Helpers;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Nexora.Services.Common.Middlewares
{
    public class DecryptIdParametersMiddleware(
    RequestDelegate _next,
    IOptions<EncryptionConfigurationModel> _configuration,
    ILogger<DecryptIdParametersMiddleware> _logger)
    {
        private readonly EncryptionConfigurationModel.ConfigurationModel? _idEncryptionConfig =
            _configuration.Value.EncryptionConfigurations?.FirstOrDefault(x => x.Name == "IdEncryption");

        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> EncryptedPropsCache = new();

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.HasValue && _idEncryptionConfig != null)
            {
                if (context.Request.Query.Count > 0)
                {
                    DecryptIdInQueryParameters(context);
                }

                if (context.Request.RouteValues.Count > 0)
                {
                    DecryptIdInRouteParameters(context);
                }

                if (HttpMethods.IsPost(context.Request.Method) &&
                    context.Request.ContentType?.Contains("application/json", StringComparison.OrdinalIgnoreCase) == true)
                {
                    await DecryptIdInBody(context);
                }

                if (context.Request.HasFormContentType)
                {
                    await DecryptIdInForm(context);
                }
            }

            await _next(context);
        }

        private void DecryptIdInQueryParameters(HttpContext context)
        {
            var newQuery = new Dictionary<string, StringValues>(context.Request.Query);

            foreach (var query in context.Request.Query
                         .Where(x =>
                             (x.Key.EndsWith("id", StringComparison.OrdinalIgnoreCase) ||
                              x.Key.EndsWith("ids", StringComparison.OrdinalIgnoreCase)) &&
                             !string.IsNullOrEmpty(x.Value) &&
                             !long.TryParse(x.Value, out _)))
            {
                var decryptedValues = query.Value
                    .Select(item =>
                    {
                        try
                        {
                            var decryptedId = EncryptionHelper.DecryptNumeric(item, _idEncryptionConfig!.Key ?? "", _idEncryptionConfig.IV ?? "");

                            return decryptedId?.ToString() ?? item;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Query param decrypt edilemedi. Key: {Key}, Value: {Value}", query.Key, item);

                            return item;
                        }
                    })
                    .ToArray();

                newQuery[query.Key] = new StringValues(decryptedValues);
            }

            context.Request.QueryString = QueryString.Create(newQuery);
        }

        private void DecryptIdInRouteParameters(HttpContext context)
        {
            foreach (var route in context.Request.RouteValues
                         .Where(x =>
                             x.Key.EndsWith("id", StringComparison.OrdinalIgnoreCase) &&
                             x.Value != null &&
                             !long.TryParse(x.Value.ToString(), out _)))
            {
                try
                {
                    var decryptedValue = EncryptionHelper.DecryptNumeric(route.Value!.ToString()!, _idEncryptionConfig!.Key ?? "", _idEncryptionConfig.IV ?? "");

                    if (decryptedValue != null)
                    {
                        context.Request.RouteValues[route.Key] = decryptedValue.ToString();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Route param decrypt edilemedi. Key: {Key}, Value: {Value}", route.Key, route.Value);
                }
            }
        }

        private async Task DecryptIdInBody(HttpContext context)
        {
            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(body))
            {
                context.Request.Body.Position = 0;
                return;
            }

            var endpoint = context.GetEndpoint();
            var actionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();

            var modelType = actionDescriptor?.Parameters
                .FirstOrDefault(p => p.BindingInfo?.BindingSource == BindingSource.Body)
                ?.ParameterType;

            if (modelType == null)
            {
                context.Request.Body.Position = 0;
                return;
            }

            var encryptedProps = EncryptedPropsCache.GetOrAdd(modelType,
                t => [.. t.GetProperties().Where(p => Attribute.IsDefined(p, typeof(EncryptedIdAttribute)))]);

            if (encryptedProps.Length == 0)
            {
                context.Request.Body.Position = 0;
                return;
            }

            var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (dict == null)
            {
                context.Request.Body.Position = 0;
                return;
            }

            foreach (var prop in encryptedProps)
            {
                var jsonKey = prop.Name;

                if (dict.TryGetValue(jsonKey, out var value) && value.ValueKind == JsonValueKind.String)
                {
                    var strVal = value.GetString();
                    if (!string.IsNullOrEmpty(strVal) && !long.TryParse(strVal, out _))
                    {
                        try
                        {
                            var decrypted = EncryptionHelper.DecryptNumeric(strVal, _idEncryptionConfig!.Key ?? "", _idEncryptionConfig.IV ?? "");

                            if (decrypted != null)
                            {
                                dict[jsonKey] = JsonDocument.Parse(decrypted.Value.ToString()).RootElement;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Body param decrypt edilemedi. Property: {Property}, Value: {Value}", jsonKey, strVal);
                        }
                    }
                }
            }

            var updatedJson = JsonSerializer.Serialize(dict);
            var bytes = Encoding.UTF8.GetBytes(updatedJson);

            context.Request.Body = new MemoryStream(bytes);
            context.Request.ContentLength = bytes.Length;
        }

        private async Task DecryptIdInForm(HttpContext context)
        {
            var form = await context.Request.ReadFormAsync();

            var dict = new Dictionary<string, StringValues>(form);

            foreach (var field in form.Where(x =>
                         (x.Key.EndsWith("id", StringComparison.OrdinalIgnoreCase) ||
                          x.Key.EndsWith("ids", StringComparison.OrdinalIgnoreCase)) &&
                         !string.IsNullOrEmpty(x.Value) &&
                         !long.TryParse(x.Value, out _)))
            {
                try
                {
                    var decryptedValues = field.Value
                        .Select(item =>
                        {
                            try
                            {
                                var decryptedId = EncryptionHelper.DecryptNumeric(item, _idEncryptionConfig!.Key ?? "", _idEncryptionConfig.IV ?? "");

                                return decryptedId?.ToString() ?? item;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Form param decrypt edilemedi. Key: {Key}, Value: {Value}", field.Key, item);
                                return item;
                            }
                        })
                        .ToArray();

                    dict[field.Key] = new StringValues(decryptedValues);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Form param decrypt edilemedi. Key: {Key}, Value: {Value}", field.Key, field.Value);
                }
            }

            context.Request.Form = new FormCollection(dict, form.Files);
        }
    }

}
