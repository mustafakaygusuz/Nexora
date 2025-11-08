using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nexora.Core.Common.Configurations;
using Nexora.Core.Common.Extensions;
using Nexora.Core.Common.Helpers;

namespace Nexora.Core.Common.Attributes
{
    public sealed class EncryptIdForCreatedResultAttribute : ActionFilterAttribute
    {
        private readonly EncryptionConfigurationModel _configuration;

        public EncryptIdForCreatedResultAttribute(EncryptionConfigurationModel configuration)
        {
            _configuration = configuration;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is CreatedResult)
            {
                var result = context.Result as CreatedResult;

                if (result.HasValue() && !string.IsNullOrEmpty(result.Value.ToString()) && long.TryParse(result.Value.ToString(), out long createdId))
                {
                    var encryptionConfig = _configuration?.EncryptionConfigurations?.FirstOrDefault(x => x.Name == "IdEncryption");

                    result.Value = EncryptionHelper.EncryptNumeric(createdId, encryptionConfig?.Key ?? "", encryptionConfig?.IV ?? "");
                }
            }

            base.OnResultExecuting(context);
        }
    }
}