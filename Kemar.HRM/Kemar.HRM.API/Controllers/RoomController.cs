//using Kemar.HRM.Business.RoomBusiness;
//using Kemar.HRM.Model.Request;
//using Microsoft.AspNetCore.Mvc;

//namespace Kemar.HRM.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class RoomController : ControllerBase
//    {
//        private readonly IRoomManager _roomManager;

//        public RoomController(IRoomManager roomManager)
//        {
//            _roomManager = roomManager;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            return Ok(await _roomManager.GetAllAsync());
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var result = await _roomManager.GetByIdAsync(id);   
//            if(result == null) return NotFound();
//            return Ok(result);

//        }

//        [HttpGet("available")]
//        public async Task<IActionResult> GetAvailable()
//        {
//            return Ok(await _roomManager.GetAvailableRoomAsync());
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create(RoomRequest request)
//        {
//            var result = await _roomManager.CreateAsync(request);
//            return Ok(result);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, RoomRequest request)
//        {
//            var result = await _roomManager.UpdateAsync(id,request);
//            if(result == null) return NotFound();
//            return Ok(result);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            return Ok(await _roomManager.DeleteAsync(id));
//        }

//        //[HttpPost("allocation")]
//        //public async Task<IActionResult> Allocate(int studentId, int roomId)
//        //{
//        //    return Ok(await _roomManager.AllocateRoomAsync(studentId, roomId));
//        //}

//        //[HttpPost("free")]
//        //public async Task<IActionResult> Free(int studentId, int roomId)
//        //{
//        //    return Ok(await _roomManager.FreeRoomAsync(studentId, roomId));
//        //}
//    }
//}
