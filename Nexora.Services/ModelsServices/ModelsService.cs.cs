using Microsoft.EntityFrameworkCore;
using Nexora.Core.Common.Enumerations;
using Nexora.Data.Domain.DbContexts;
using Nexora.Services.ModelsServices.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexora.Services.ModelsServices
{
    public class ModelsService : IModelsService
    {
        private readonly ApplicationDbContext _context;
        public ModelsService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// List models by brand id
        /// </summary>
        public async Task<List<ModelsListByBrandIdResult>> ListByBrandId(long brandId)
        {
            return await _context.Models
                .AsNoTracking()
                .Where(x =>
                    x.BrandId == brandId &&
                    x.Status == StatusType.Active)
                .OrderBy(x => x.Name)
                .Select(x => new ModelsListByBrandIdResult
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }
    }
}
