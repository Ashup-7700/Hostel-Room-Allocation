using Kemar.HRM.Model.Common;

namespace Kemar.HRM.Business.UserTokenBusiness
{
     public interface IUserTokenManager
    {
        Task<ResultModel> CreateTokenAsync(int userId, string token, string? systemIp, DateTime expiresAt);
        Task<ResultModel> GetTokenByUserIdAsync(int userId);
        Task<ResultModel> DeleteTokenAsync(int userTokenId);
    }
}
