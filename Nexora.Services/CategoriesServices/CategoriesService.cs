using Microsoft.EntityFrameworkCore;
using Nexora.Core.Common.Enumerations;
using Nexora.Data.Domain.DbContexts;
using Nexora.Services.CategoriesServices.Dtos.Response;

namespace Nexora.Services.CategoriesServices
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationDbContext _context;

        public CategoriesService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// List categories
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoriesListResult>> List()
        {
            return await _context.Categories
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Where(x => x.Status == StatusType.Active)
                .Select(x => new CategoriesListResult
                {
                    Id = x.Id,
                    Type = x.Type,
                    Name = x.Name,
                    Status = x.Status
                })
                .ToListAsync();
        }
    }
}