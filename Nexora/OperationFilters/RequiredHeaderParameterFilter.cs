using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Nexora.Api.OperationFilters
{
    public sealed class RequiredHeaderParameterFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= [];

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Language-Id",
                In = ParameterLocation.Header,
                Description = "Example: tr, en",
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString("tr") // varsayılan değer
                }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Channel-Type",
                In = ParameterLocation.Header,
                Description = "Example: Ios, Android, Web",
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString("Ios") // varsayılan değer
                }
            });
        }
    }
}