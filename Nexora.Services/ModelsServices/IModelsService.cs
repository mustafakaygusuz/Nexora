using Nexora.Services.ModelsServices.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexora.Services.ModelsServices
{
    public interface IModelsService
    {
        /// <summary>
        /// List models by brand id
        /// </summary>
        Task<List<ModelsListByBrandIdResult>> ListByBrandId(long brandId);
    }
}
