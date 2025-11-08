using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Nexora.Core.Common.Models;
using System.Reflection;

namespace Nexora.Core.Common.Helpers
{
    public static class EndpointScanHelper
    {
        public static List<EndpointScanInfoModel> ScanEndpoints()
        {
            var result = new List<EndpointScanInfoModel>();

            var controllerTypes = Assembly.GetEntryAssembly()!.GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToList();

            foreach (var controllerType in controllerTypes)
            {
                var routeAttributes = controllerType.GetCustomAttributes<RouteAttribute>();
                var apiVersionAttribute = controllerType.GetCustomAttribute<ApiVersionAttribute>();

                var version = apiVersionAttribute?.Versions.FirstOrDefault()?.ToString() ?? "1";

                if (version.EndsWith(".0"))
                {
                    version = version.Substring(0, version.Length - 2);
                }

                foreach (var method in controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute))))
                {
                    var httpMethodAttributes = method.GetCustomAttributes<HttpMethodAttribute>(false);

                    foreach (var httpMethodAttribute in httpMethodAttributes)
                    {
                        var routeTemplate = routeAttributes.Any() ? routeAttributes.First().Template : string.Empty;
                        var methodRouteTemplate = httpMethodAttribute.Template;

                        routeTemplate = CombineRouteTemplates(routeTemplate, methodRouteTemplate);

                        routeTemplate = routeTemplate.Replace("[controller]", controllerType.Name.Replace("Controller", string.Empty))
                            .Replace("[action]", string.Empty)
                            .Replace("{version:apiVersion}", version);

                        var endpointInfo = new EndpointScanInfoModel
                        {
                            HttpMethod = httpMethodAttribute.HttpMethods.FirstOrDefault() ?? "GET",
                            Url = $"/{GeneratedRegexHelper.EndpointScanUrlParameterRegex().Replace(routeTemplate, "{parameter}").ToLower()}"
                        };

                        result.Add(endpointInfo);
                    }
                }
            }

            return result;
        }

        private static string CombineRouteTemplates(string controllerRoute, string actionRoute)
        {
            if (string.IsNullOrEmpty(controllerRoute))
            {
                return actionRoute;
            }

            if (string.IsNullOrEmpty(actionRoute))
            {
                return controllerRoute;
            }

            return $"{controllerRoute.TrimEnd('/')}/{actionRoute.TrimStart('/')}";
        }
    }
}