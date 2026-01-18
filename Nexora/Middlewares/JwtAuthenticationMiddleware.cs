using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Nexora.Core.Common.Enumerations;
using Nexora.Core.Common.Extensions;
using Nexora.Core.Contexts;
using Nexora.Data.SystemConfigurationManagers;
using Nexora.Security.JWT;
using Nexora.Services.ConsumersServices;
using System.Security.Claims;

namespace Nexora.Api.Middlewares
{
    public class JwtAuthenticationMiddleware(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context, ISystemConfigurationManager systemConfigurationManagers, IConsumersService consumersService, ApiContext apiContext, ITokenHelper tokenHelper)
        {
            if (!SkipEndpoint(context))
            {
                var tokenResult = await CheckToken(context, tokenHelper);
                if (tokenResult.IsValid.HasValue())
                {
                    if (tokenResult.IsValid!.Value)
                    {
                        var consumer = await consumersService.GetById(tokenResult.ConsumerId!.Value);
                        if (consumer.HasValue() && consumer!.Status == StatusType.Active && consumer.AccessToken == tokenResult.AccessToken)
                        {
                            apiContext.ConsumerId = consumer.Id;
                        }
                        else
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return;
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
            }
            await _next(context);
        }

        private static bool SkipEndpoint(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null ||
                endpoint.Metadata.OfType<IAllowAnonymous>().Any() ||
                !endpoint.Metadata.OfType<IAuthorizeData>().Any())
            {
                return true;
            }
            return false;
        }

        private static Task<(bool? IsValid, string? AccessToken, long? ConsumerId)> CheckToken(HttpContext context, ITokenHelper tokenHelper)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var accessToken = authHeader.ToString().Replace("Bearer", "", StringComparison.OrdinalIgnoreCase).Trim();

                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    Console.WriteLine("Token is empty");
                    return Task.FromResult<(bool? IsValid, string? AccessToken, long? ConsumerId)>(default);
                }

                Console.WriteLine($"Validating token: {accessToken.Substring(0, Math.Min(50, accessToken.Length))}...");

                var principal = tokenHelper.ValidateToken(accessToken);

                if (principal == null)
                {
                    Console.WriteLine("Principal is NULL - Token validation failed!");
                    return Task.FromResult<(bool? IsValid, string? AccessToken, long? ConsumerId)>((false, null, null));
                }

                Console.WriteLine($"Principal claims count: {principal.Claims?.Count()}");

                var consumerIdClaim = principal.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (consumerIdClaim != null && long.TryParse(consumerIdClaim.Value, out var consumerId))
                {
                    Console.WriteLine($"Consumer ID found: {consumerId}");
                    var identity = new ClaimsIdentity(principal.Claims, JwtBearerDefaults.AuthenticationScheme);
                    context.User = new ClaimsPrincipal(identity);
                    return Task.FromResult<(bool? IsValid, string? AccessToken, long? ConsumerId)>((true, accessToken, consumerId));
                }

                Console.WriteLine("Consumer ID claim not found or invalid");
                return Task.FromResult<(bool? IsValid, string? AccessToken, long? ConsumerId)>((false, null, null));
            }

            Console.WriteLine("Authorization header not found");
            return Task.FromResult<(bool? IsValid, string? AccessToken, long? ConsumerId)>(default);
        }
    }

}