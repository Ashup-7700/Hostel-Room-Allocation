using Kemar.HRM.API.Helpers;
using Kemar.HRM.Business.RoomAllocationBusiness;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kemar.HRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomAllocationController : ControllerBase
    {
        private readonly IRoomAllocationManager _manager;

        public RoomAllocationController(IRoomAllocationManager manager)
        {
            _manager = manager;
        }

        [Authorize (Roles = "Hostel Manager,Warden")]
        [HttpPost("addOrUpdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] RoomAllocationRequest request)
        {
            var result = await _manager.AddOrUpdateAsync(request);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [Authorize]
        [HttpGet("getById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _manager.GetByIdAsync(id);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [Authorize]
        [HttpPost("getByFilter")]
        public async Task<IActionResult> GetByFilter([FromBody] RoomAllocationFilter filter)
        {
            var result = await _manager.GetByFilterAsync(filter);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [Authorize]
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _manager.DeleteAsync(id);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }
    }
}
