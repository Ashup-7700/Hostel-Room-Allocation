//using Kemar.HRM.Business.RoomAllocationBusiness;
//using Kemar.HRM.Model.Request;
//using Microsoft.AspNetCore.Mvc;

//namespace Kemar.HRM.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class RoomAllocationController : ControllerBase
//    {
//        private readonly IRoomAllocationManager _manager;

//        public RoomAllocationController(IRoomAllocationManager manager)
//        {
//            _manager = manager;
//        }

//        [HttpPost("allocate")]
//        public async Task<IActionResult> AllocateRoom(RoomAllocationRequest request)
//        {
//            var result = await _manager.AllocateRoomAsync(request);
//            return Ok(result);
//        }

//        [HttpPut("free/{allocationId}")]
//        public async Task<IActionResult> FreeRoom(int allocationId)
//        {
//            var result = await _manager.FreeRoomAsync(allocationId);
//            return Ok(result);
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            return Ok(await _manager.GetAllAsync());
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> Get(int id)
//        {
//            var record = await _manager.GetByIdAsync(id);
//            return record == null ? NotFound() : Ok(record);
//        }

//        [HttpGet("student/{studentId}")]
//        public async Task<IActionResult> GetByStudent(int studentId)
//        {
//            return Ok(await _manager.GetByStudentAsync(studentId));
//        }

//        [HttpGet("room/{roomId}")]
//        public async Task<IActionResult> GetByRoom(int roomId)
//        {
//            return Ok(await _manager.GetByRoomAsync(roomId));
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var deleted = await _manager.DeleteAsync(id);
//            return deleted ? Ok("Deleted successfully") : NotFound();
//        }
//    }
//}
