using Kemar.HRM.API.Helpers;
using Kemar.HRM.Business.RoomAllocationBusiness;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace Kemar.HRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomAllocationController : ControllerBase
    {
        private readonly IRoomAllocationManager _manager;

        public RoomAllocationController(IRoomAllocationManager manager)
        {
            _manager = manager;
        }

        [HttpPost("addOrUpdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] RoomAllocationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { statusCode = 400, message = "Invalid data model" });

            CommonHelper.SetUserInformation(ref request, request.RoomAllocationId ?? 0, HttpContext);

            if (!request.AllocatedByUserId.HasValue)
            {

            }

            var result = await _manager.AddOrUpdateAsync(request);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [HttpGet("getById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _manager.GetByIdAsync(id);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [HttpPost("getByFilter")]
        public async Task<IActionResult> GetByFilter([FromBody] RoomAllocationFilter filter)
        {
            var result = await _manager.GetByFilterAsync(filter);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var username = HttpContext.User?.Identity?.Name ?? "admin";
            var result = await _manager.DeleteAsync(id, username);
            return CommonHelper.ReturnActionResultByStatus(result, this);

        }
    }
}
