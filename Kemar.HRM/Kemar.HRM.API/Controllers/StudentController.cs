using Kemar.HRM.API.Helpers;
using Kemar.HRM.Business.StudentBusiness;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace Kemar.HRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentManager _studentManager;

        public StudentController(IStudentManager studentManager)
        {
            _studentManager = studentManager;
        }

        [HttpPost("addOrUpdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] StudentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { statusCode = 400, message = "Invalid data model" });

            CommonHelper.SetUserInformation(ref request, request.StudentId ?? 0, HttpContext);

            var result = await _studentManager.AddOrUpdateAsync(request);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [HttpGet("getById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _studentManager.GetByIdAsync(id);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [HttpPost("getByFilter")]
        public async Task<IActionResult> GetByFilter([FromBody] StudentFilter filter)
        {
            var result = await _studentManager.GetByFilterAsync(filter);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var username = HttpContext.User?.Identity?.Name ?? "admin";
            var result = await _studentManager.DeleteAsync(id, username);
            return CommonHelper.ReturnActionResultByStatus(result, this);

        }
    }
}
