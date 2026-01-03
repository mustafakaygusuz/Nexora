using Microsoft.EntityFrameworkCore;
using Nexora.Core.Common.Enumerations;
using Nexora.Data.Domain.DbContexts;
using Nexora.Services.BrandsServices.Dtos.Response;

namespace Nexora.Services.BrandsServices
{
    public class BrandsService : IBrandsService
    {
        private readonly ApplicationDbContext _context;
        public BrandsService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// List brands by category id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<BrandsListByCategoryIdResult>> ListByCategoryId(long categoryId)
        {
            return await _context.Brands
            .AsNoTracking()
            .Where(x =>
                x.CategoryId == categoryId &&
                x.Status == StatusType.Active)
            .OrderBy(x => x.Name)
            .Select(x => new BrandsListByCategoryIdResult
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();

        }
    }
}