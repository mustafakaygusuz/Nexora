using Nexora.Data.Domain.Entities;

namespace Nexora.Data.ConsumersRepositories
{
    public interface IConsumersRepository
    {
        Task<Consumer> GetByEmailAsync(string email);
        Task<Consumer> GetByIdAsync(long id);
        Task<long> InsertAsync(Consumer consumer);
        Task<int> UpdateAsync(Consumer consumer);
    }
}