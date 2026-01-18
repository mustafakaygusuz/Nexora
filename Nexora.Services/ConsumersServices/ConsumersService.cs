using Microsoft.EntityFrameworkCore;
using Nexora.Core.Caching.Constants;
using Nexora.Core.Caching.Services;
using Nexora.Core.Common.Enumerations;
using Nexora.Core.Common.Exceptions;
using Nexora.Core.Common.Extensions;
using Nexora.Core.Contexts;
using Nexora.Data.Domain.DbContexts;
using Nexora.Data.Domain.Entities;
using Nexora.Services.ConsumersServices.Dtos.Response;
using System.Net;

namespace Nexora.Services.ConsumersServices
{
    public class ConsumersService(ApiContext _apiContext, ApplicationDbContext _context, ICacheService _cacheService) : IConsumersService
    {
        /// <summary>
        /// Delete consuer account
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpStatusCodeException"></exception>
        public async Task DeleteAccount()
        {
            if (!_apiContext.ConsumerId.HasValue())
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, StaticTextKeyType.CnsmrExNtFnd);
            }
            var consumer = await _context.Consumers.FirstOrDefaultAsync(x => x.Id == _apiContext.ConsumerId && x.Status == StatusType.Active);

            if (!consumer.HasValue())
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, StaticTextKeyType.CnsmrExNtFnd);
            }

            consumer!.Status = StatusType.Deleted;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get profile by curent consumer
        /// </summary>
        /// <returns></returns>
        public async Task<ConsumersGetProfileResult> GetProfile()
        {
            if (!_apiContext.ConsumerId.HasValue())
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, StaticTextKeyType.CnsmrExNtFnd);
            }

            var consumer = await _context.Consumers.FirstOrDefaultAsync(x => x.Id == _apiContext.ConsumerId && x.Status == StatusType.Active);

            var result = new ConsumersGetProfileResult()
            {
                Name = consumer!.Name,
                Surname = consumer.Surname,
                Nickname = consumer.Nickname,
                Email = consumer!.Email,
                Gender = consumer?.Gender,
                BirthDate = consumer?.BirthDate,
                Description = consumer?.Description
            };

            return result;
        }

        /// <summary>
        /// Get consumer by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Consumer?> GetById(long id)
        {
            return await _cacheService.GetAndSetHash(
                CacheKeys.ConsumersDataHashId(id),
                CacheKeys.ConsumersDetail,
                async () => await _context.Consumers
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Id == id)
                , (_apiContext.CurrentDate.AddDays(1) - _apiContext.CurrentDate).TotalSeconds.ToInt());
        }
    }
}