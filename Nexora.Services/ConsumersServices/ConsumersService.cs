using Microsoft.EntityFrameworkCore;
using Nexora.Core.Common.Enumerations;
using Nexora.Core.Common.Exceptions;
using Nexora.Core.Common.Extensions;
using Nexora.Core.Contexts;
using Nexora.Data.ConsumersRepositories;
using Nexora.Data.Domain.DbContexts;
using Nexora.Services.ConsumersServices.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nexora.Services.ConsumersServices
{
    public class ConsumersService(ApiContext _apiContext, ApplicationDbContext _context) : IConsumersService
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
    }
}