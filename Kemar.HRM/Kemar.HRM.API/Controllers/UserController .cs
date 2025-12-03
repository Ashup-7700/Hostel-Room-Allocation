using Kemar.HRM.API.Helpers;
using Kemar.HRM.Business.UserBusiness;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace Kemar.HRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("addOrUpdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { statusCode = 400, message = "Invalid data model" });

            CommonHelper.SetUserInformation(ref request, request.UserId ?? 0, HttpContext);

            var result = await _userManager.AddOrUpdateAsync(request);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [HttpGet("getById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userManager.GetByIdAsync(id);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [HttpPost("getByFilter")]
        public async Task<IActionResult> GetByFilter([FromBody] UserFilter filter)
        {
            var result = await _userManager.GetByFilterAsync(filter);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            CommonHelper.SetUserInformation(ref id, id, HttpContext); 
            var username = HttpContext.User?.Identity?.Name ?? "admin";
            var result = await _userManager.DeleteAsync(id, username);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }
    }
}
