using Kemar.HRM.Business.RoomAllocationBusiness;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Kemar.HRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // just need the user to be logged in
    public class RoomAllocationController : ControllerBase
    {
        private readonly IRoomAllocationManager _manager;

        public RoomAllocationController(IRoomAllocationManager manager)
        {
            _manager = manager;
        }

        [HttpPost("addOrUpdate")]
        public async Task<IActionResult> AddOrUpdateRoomAllocation(RoomAllocationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

  
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out int currentUserId))
            {
                request.AllocatedByUserId = currentUserId;
            }

            var result = await _manager.AddOrUpdateAsync(request);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _manager.GetByIdAsync(id);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [HttpPost("GetByFilter")]
        public async Task<IActionResult> GetByFilter([FromBody] RoomAllocationFilter filter)
        {
            var result = await _manager.GetByFilterAsync(filter);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _manager.DeleteAsync(id);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }
    }
}
