using Nexora.Core.Common.Models;

namespace Nexora.Services.ConsumersServices
{
    public interface IConsumersService
    {
        ///// <summary>
        ///// Insert a new consumer into the database.
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        ///// <exception cref="HttpStatusCodeException"></exception>
        //Task<(long Id, Guid UniqueId)> Insert(ConsumersInsertRequest request);

        ///// <summary>
        ///// Update consumer tokens
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="accessToken"></param>
        ///// <param name="refreshToken"></param>
        ///// <returns></returns>
        //Task UpdateTokens(long id, string accessToken, string refreshToken = "");

        ///// <summary>
        ///// Get consumer by phone number
        ///// </summary>
        ///// <param name="phoneNumber"></param>
        ///// <returns></returns>
        ///// <exception cref="HttpStatusCodeException"></exception>
        //Task<ConsumersGetByPhoneNumberResult?> GetByPhoneNumber(string phoneNumber);

        ///// <summary>
        ///// Get consumer by phone number
        ///// </summary>
        ///// <param name="email"></param>
        ///// <returns></returns>
        ///// <exception cref="HttpStatusCodeException"></exception>
        //Task<long?> GetIdByEmail(string email);

        ///// <summary>
        ///// Check if a consumer exists by email or phone number.
        ///// </summary>
        ///// <param name="phoneNumber"></param>
        ///// <param name="email"></param>
        ///// <returns></returns>
        //Task<bool> CheckExistByEmailOrPhoneNumber(string phoneNumber, string? email);

        ///// <summary>
        ///// Check if the registration limit has expired for a given phone number.
        ///// </summary>
        ///// <param name="phoneNumber"></param>
        ///// <returns></returns>
        //Task<bool> IsRegisterLimitExpired(string phoneNumber);

        ///// <summary>
        ///// Get consumer by Id.
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //Task<Consumer?> GetById(long id);

        ///// <summary>
        ///// Update consumer consent.
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //Task<bool> UpdateConsent(long id, List<ConsumersUpdateConsentRequest> request, bool sendToIys = false);

        ///// <summary>
        ///// Get consumer profile by consumer id
        ///// </summary>
        ///// <returns></returns>
        ///// <exception cref="HttpStatusCodeException"></exception>
        //Task<ConsumersGetProfileResult> GetProfile();

        ///// <summary>
        ///// Update consumer profile
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        ///// <exception cref="HttpStatusCodeException"></exception>
        //Task UpdateProfile(ConsumersUpdateProfileRequest request);

        ///// <summary>
        ///// Get consumer consents by consumer id
        ///// </summary>
        ///// <returns></returns>
        ///// <exception cref="HttpStatusCodeException"></exception>
        //Task<List<ConsumersListConsentsResult>?> GetConsents();

        ///// <summary>
        ///// Update consumer consents
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        ///// <exception cref="HttpStatusCodeException"></exception>
        //Task UpdateConsents(List<ConsumersUpdateConsentsRequest> request);

        ///// <summary>
        ///// Consumer logout
        ///// </summary>
        ///// <returns></returns>
        //Task Logout();

        ///// <summary>
        ///// Delete account
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        ///// <exception cref="HttpStatusCodeException"></exception>
        //Task DeleteAccount(ConsumersDeleteAccountRequest request);

        ///// <summary>
        ///// Get total member count by status type
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //Task<long> GetTotalMemberCountByStatus(StatusType request);

        ///// <summary>
        ///// Get register, delete account and difference between them report by date range
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //Task<List<ConsumersRegistrationDeletionReportResult>> GetConsumerRegistrationDeletionReport(ConsumersRegistrationDeletionReportRequest request);

        ///// <summary>
        ///// List consumers by ConsumersSearchRequest
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //Task<SearchResultModel<ConsumersSearchResult>> Search(ConsumersSearchRequest request);

        ///// <summary>
        ///// Get detail by id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns>for panel consumer info</returns>
        //Task<ConsumersGetDetailByIdResult?> GetDetailById(long id);

        ///// <summary>
        ///// Delete consumer by id
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        ///// <exception cref="HttpStatusCodeException"></exception>
        //Task DeleteById(long id);
    }
}
