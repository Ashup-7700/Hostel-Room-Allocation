using Kemar.HRM.Business.UserBusiness;
using Kemar.HRM.Business.UserTokenBusiness;
using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Kemar.HRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IUserTokenManager _tokenManager;
        private readonly IConfiguration _config;

        public UserController(IUserManager userManager, IUserTokenManager tokenManager, IConfiguration config)
        {
            _userManager = userManager;
            _tokenManager = tokenManager;
            _config = config;
        }

        [HttpPost("addOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid model");

            var result = await _userManager.AddOrUpdateAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("getById/{userId}")]
        public async Task<IActionResult> GetByIdAsync(int userId)
        {
            var result = await _userManager.GetByIdAsync(userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("getByFilter")]
        public async Task<IActionResult> GetByFilterAsync([FromBody] UserFilter filter)
        {
            var result = await _userManager.GetByFilterAsync(filter);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteAsync(int userId)
        {
            var result = await _userManager.DeleteAsync(userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginRequest request)
        {
            var userResult = await _userManager.AuthenticateAsync(request);


            if (!userResult.IsSuccess)
                return StatusCode((int)userResult.StatusCode, userResult);

            var user = (dynamic)userResult.Data;

            string token = GenerateJwtToken(user);

            DateTime generatedAt = DateTime.UtcNow;
            DateTime expiresAt = generatedAt.AddMinutes(40);
            string? systemIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            await _tokenManager.CreateTokenAsync(
                user.UserId,
                token,
                systemIp,
                expiresAt
            );

            var response = new UserLoginResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                Role = user.Role,
                Token = token,
                GeneratedAt = generatedAt,
                ExpiresAt = expiresAt,
                SystemIp = systemIp
            };

            return Ok(ResultModel.Success(response, "Login Successful"));
        }

        private string GenerateJwtToken(dynamic user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userId", user.UserId.ToString()),
                new Claim("username", user.Username),
                new Claim("role", user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
