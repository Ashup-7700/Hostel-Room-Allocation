using Kemar.HRM.Business.StudentBusiness;
using Kemar.HRM.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace Kemar.HRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentManager _studentManager;

        public StudentController(IStudentManager studentManager)
        {
            _studentManager = studentManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _studentManager.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _studentManager.GetByIdAsync(id);

            if (result == null)
                return NotFound("Student not found");

            return Ok(result);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name is required.");

            var result = await _studentManager.GetByNameAsync(name);

            if (!result.Any())
                return NotFound("No students found with the given name.");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _studentManager.CreateAsync(request);

            return Ok(created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] StudentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _studentManager.UpdateAsync(id, request);

            if (updated == null)
                return NotFound("Student not found");

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _studentManager.DeleteAsync(id);

            if (!deleted)
                return NotFound("Student not found");

            return Ok(new { message = "Student deleted successfully" });
        }
    }
}
















// GET: api/student/with-allocations/5
//[HttpGet("with-allocations/{id:int}")]
//public async Task<IActionResult> GetWithAllocations(int id)
//{
//    var result = await _studentManager.GetStudentWithAllocations(id);

//    if (result == null)
//        return NotFound("Student not found");

//    return Ok(result);
//}

//// GET: api/student/with-payments/5
//[HttpGet("with-payments/{id:int}")]
//public async Task<IActionResult> GetWithPayments(int id)
//{
//    var result = await _studentManager.GetStudentWithPayments(id);

//    if (result == null)
//        return NotFound("Student not found");

//    return Ok(result);
//}