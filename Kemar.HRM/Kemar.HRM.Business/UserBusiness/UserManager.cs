using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;
using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.Interface;
using Kemar.HRM.Business.UserTokenBusiness;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Kemar.HRM.Business.UserBusiness
{
    public class UserManager : IUserManager
    {
        private readonly IUser _userRepo;
        private readonly IUserTokenManager _userTokenManager;
        private readonly IConfiguration _config;

        public UserManager(IUser userRepo, IUserTokenManager userTokenManager, IConfiguration config)
        {
            _userRepo = userRepo;
            _userTokenManager = userTokenManager;
            _config = config;
        }

        public async Task<ResultModel> AddOrUpdateAsync(UserRequest request)
        {
            return await _userRepo.AddOrUpdateAsync(request);
        }

        public async Task<ResultModel> GetByFilterAsync(UserFilter filter)
        {
            return await _userRepo.GetByFilterAsync(filter);
        }

        public async Task<ResultModel> AuthenticateAsync(UserLoginRequest request)
        {
            var user = await _userRepo.AuthenticateAsync(request.Username, request.Password);

            if (user == null)
                return ResultModel.Failure(ResultCode.Invalid, "Invalid username or password");

            var token = GenerateJwtToken(user);
            var generatedAt = DateTime.UtcNow;
            var expiresAt = generatedAt.AddMinutes(40); 

            await _userTokenManager.CreateTokenAsync(user.UserId, token, GetClientIp(), expiresAt);

            var response = new UserLoginResponse
            {
                Token = token,
                Username = user.Username,
                FullName = user.FullName,
                Role = user.Role,
                GeneratedAt = generatedAt,
                ExpiresAt = expiresAt,
                SystemIp = GetClientIp()
            };

            return ResultModel.Success(response, "Authentication successful");
        }

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"] ?? "ThisIsASecretKey12345");
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(40),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<ResultModel> GetByIdAsync(int userId)
        {
            return await _userRepo.GetByIdAsync(userId);
        }

        public async Task<ResultModel> DeleteAsync(int userId)
        {
            var result = await _userRepo.DeleteAsync(userId);
            if (result.StatusCode == ResultCode.RecordNotFound)
                return result;

            return ResultModel.Success(null, "User deleted successfully");
        }

        private string GetClientIp()
        {
            return "127.0.0.1"; 
        }
    }
}
