using Microsoft.EntityFrameworkCore;
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
        public async Task<List<CategoriesListResult>> List()
        {
            return await _context.Categories
                .AsNoTracking()
                .OrderBy(x => x.Id)
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