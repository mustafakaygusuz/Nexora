using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nexora.Core.Common.Enumerations;
using Nexora.Core.Common.Exceptions;
using Nexora.Core.Common.Extensions;
using Nexora.Core.Common.Helpers;
using Nexora.Core.Common.Models;
using Nexora.Core.Contexts;
using Nexora.Data.Domain.Enumerations;
using Nexora.Data.StaticTextsManagers;
using Serilog.Context;
using System.Net;
using System.Text.Json;

namespace Nexora.Services.Common.Middlewares
{
    public class ErrorHandlerMiddleware(RequestDelegate _next)
    {
        public async Task Invoke(HttpContext context, ILogger<ErrorHandlerMiddleware> _logger, IStaticTextsManager _staticTextsManager, ApiContext _apiContext)
        {
            try
            {
                await _next(context);
            }
            catch (HttpStatusCodeException ex)
            {
                PushRequestContextToLog(context, _apiContext);

                _logger.LogWarning(ex, ex.Message);
                context.Response.StatusCode = (int)ex.StatusCode;
                context.Response.ContentType = ex.ContentType;

                var errorDetail = await GetErrorDetail(_staticTextsManager, ex.Key ?? "", ex.MessageKeyTypes, ex.Message);

                await context.Response.WriteAsync(new ErrorResultModel
                {
                    Key = ex.Key,
                    Title = errorDetail.Title,
                    Message = errorDetail.Message
                }.ToJson(new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            }
            catch (Exception ex)
            {
                PushRequestContextToLog(context, _apiContext);

                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = HttpStatusCode.InternalServerError.ToInt();
                context.Response.ContentType = "application/json";

                var errorKey = StaticTextKeyHelper.Get(StaticTextKeyType.IntrnlSrvrErr);

                var errorDetail = await GetErrorDetail(_staticTextsManager, errorKey ?? "", null);

                await context.Response.WriteAsync(new ErrorResultModel
                {
                    Key = errorKey,
                    Title = errorDetail.Title,
                    Message = errorDetail.Message
                }.ToJson(new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            }
        }

        private static async Task<(string Title, string Message)> GetErrorDetail(IStaticTextsManager _staticTextsManager, string key, List<StaticTextKeyType>? keyList = null, string? overrideMessage = null)
        {
            string? title = "", message = "";

            var data = await _staticTextsManager.ListByType(StaticTextType.Error);

            if (data.HasValue())
            {
                var errorKey = StaticTextKeyHelper.Get(StaticTextKeyType.IntrnlSrvrErr);

                if (keyList.HasValue())
                {
                    var messages = await _staticTextsManager.ListByKeys(keyList!, StaticTextType.Error);

                    title =
                        data.FirstOrDefault(x => x.Key == $"{key}_Title")?.Value ??
                        data.FirstOrDefault(x => x.Key == $"{errorKey}_Title")?.Value;

                    message =
                        messages.HasValue() ? string.Join(", ", messages!.Select(x => x.Value)) : null ??
                        data.FirstOrDefault(x => x.Key == errorKey)?.Value;
                }
                else
                {
                    title =
                        data.FirstOrDefault(x => x.Key == $"{key}_Title")?.Value ??
                        data.FirstOrDefault(x => x.Key == $"{errorKey}_Title")?.Value;

                    message =
                        !string.IsNullOrWhiteSpace(overrideMessage) ? overrideMessage :
                        data.FirstOrDefault(x => x.Key == key)?.Value ??
                        data.FirstOrDefault(x => x.Key == errorKey)?.Value;
                }
            }

            return (title ?? "", message ?? "");
        }

        private static void PushRequestContextToLog(HttpContext context, ApiContext _apiContext)
        {
            var request = context.Request;

            LogContext.PushProperty("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true);
            LogContext.PushProperty("RequestHost", request.Host);
            LogContext.PushProperty("RequestProtocol", request.Protocol);
        }
    }
}