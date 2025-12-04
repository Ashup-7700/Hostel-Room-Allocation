using Kemar.HRM.API.Helpers;
using Kemar.HRM.Business.RoomBusiness;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace Kemar.HRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomManager _manager;

        public RoomController(IRoomManager manager)
        {
            _manager = manager;
        }

        [HttpPost("addOrUpdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] RoomRequest request)
        {
            CommonHelper.SetUserInformation(ref request, request.RoomId ?? 0, HttpContext);

            var result = await _manager.AddOrUpdateAsync(request);
            return CommonHelper.ReturnActionResultByStatus(result,this);
        }

        [HttpGet("getById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _manager.GetByIdAsync(id);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [HttpPost("getByFilter")] 
        public async Task<IActionResult> GetByFilter([FromBody] RoomFilter filter)
        {
            var result = await _manager.GetByFilterAsync(filter);
            return CommonHelper.ReturnActionResultByStatus(result,this) ;
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
