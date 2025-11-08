using Nexora.Core.Common.Configurations;
using Nexora.Core.Common.Enumerations;
using Nexora.Core.Common.Exceptions;
using Nexora.Core.Common.Extensions;
using Nexora.Core.Common.Models;
using System.Net;

namespace Nexora.Core.Common.Helpers
{
    public static class MailHashHelper
    {
        /// <summary>
        /// Check given hash
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static T CheckHash<T>(string hash, EncryptionConfigurationModel encryptionConfiguration) where T : BaseMailHashModel
        {
            try
            {
                var mailEncryption = encryptionConfiguration.EncryptionConfigurations!.First(x => x.Name == "MailEncryption");

                var hashModel = EncryptionHelper.DecryptWithAes(hash, mailEncryption.Key, mailEncryption.IV).FromJson<T>();

                if (hashModel.ExpireDate < DateTime.UtcNow)
                {
                    throw new HttpStatusCodeException(HttpStatusCode.BadRequest, StaticTextKeyType.MlHshExTknExprd);
                }

                return hashModel;
            }
            catch (Exception ex) when (ex is not HttpStatusCodeException)
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, StaticTextKeyType.MlHshExWrngHsh);
            }
        }
    }
}