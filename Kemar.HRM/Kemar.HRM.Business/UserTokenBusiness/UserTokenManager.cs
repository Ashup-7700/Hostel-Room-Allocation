using Kemar.HRM.Model.Common;
using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.Interface;

namespace Kemar.HRM.Business.UserTokenBusiness
{
    public class UserTokenManager : IUserTokenManager
    {
        private readonly IUserToken _userTokenRepository;

        public UserTokenManager(IUserToken userTokenRepository)
        {
            _userTokenRepository = userTokenRepository;
        }

        public async Task<ResultModel> CreateTokenAsync(int userId, string token, string? systemIp, DateTime expiresAt)
        {
            var newToken = new UserToken
            {
                UserId = userId,
                Token = token,
                GeneratedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt,
                SystemIp = systemIp
            };

            return await _userTokenRepository.AddAsync(newToken);
        }

        public async Task<ResultModel> DeleteTokenAsync(int userTokenId)
        {
            return await _userTokenRepository.DeleteAsync(userTokenId);
        }

        public async Task<ResultModel> GetTokenByUserIdAsync(int userId)
        {
            return await _userTokenRepository.GetByUserIdAsync(userId);
        }
    }
}
