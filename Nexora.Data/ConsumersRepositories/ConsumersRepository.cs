using Dapper;
using Microsoft.EntityFrameworkCore;
using Nexora.Core.Contexts;
using Nexora.Core.Data.DapperRepository;
using Nexora.Core.Data.EfCoreModels;
using Nexora.Data.Domain.DbContexts;
using Nexora.Data.Domain.Entities;

namespace Nexora.Data.ConsumersRepositories
{
    public sealed class ConsumersRepository(ApiContext _apiContext, IDapperRepository _dapperRepository, ApplicationDbContext _dbContext) : IConsumersRepository
    {
        public async Task<Consumer> GetByEmailAsync(string email)
        {
            return await _dbContext.Consumers.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Consumer> GetByIdAsync(long id)
        {
            return await _dbContext.Consumers.FindAsync(id);
        }

        public async Task<long> InsertAsync(Consumer consumer)
        {
            await _dbContext.Consumers.AddAsync(consumer);
            await _dbContext.SaveChangesAsync();
            return consumer.Id;
        }

        public async Task<int> UpdateAsync(Consumer consumer)
        {
            _dbContext.Consumers.Update(consumer);
            return await _dbContext.SaveChangesAsync();
        }

        public OrmResultModel<int> UpdateTokens(long id, string accessToken, string refreshToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);
            parameters.Add("AccessToken", accessToken);
            parameters.Add("RefreshToken", refreshToken);
            parameters.Add("UpdatedDate", DateTime.UtcNow);

            string query = @"UPDATE ""Consumers"" 
                     SET ""AccessToken"" = @AccessToken,
                         ""RefreshToken"" = @RefreshToken,
                         ""UpdatedDate"" = @UpdatedDate
                     WHERE ""Id"" = @Id";

            return _dapperRepository.Update(query, parameters);
        }

        public async Task<Consumer?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _dbContext.Consumers
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        }
    }
}
