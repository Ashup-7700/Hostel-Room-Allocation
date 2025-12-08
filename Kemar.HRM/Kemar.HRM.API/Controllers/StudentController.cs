using Kemar.HRM.Business.StudentBusiness;
using Kemar.HRM.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        private string LoggedInUser => User.FindFirstValue(ClaimTypes.Name) ?? "Unknown";

        // ================================
        // Create or Update - login required
        // ================================
        [HttpPost("addOrUpdate")]
        [Authorize]
        public async Task<IActionResult> AddOrUpdateAsync([FromBody] StudentRequest request)
        {
            var result = await _studentManager.AddOrUpdateAsync(request, LoggedInUser);
            return StatusCode((int)result.StatusCode, result);
        }

        // ================================
        // Get by Id - anyone can access
        // ================================
        [HttpGet("getById/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _studentManager.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        // ================================
        // Get all - login required
        // ================================
        [HttpGet("getAll")]
        [Authorize]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _studentManager.GetAllAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        // ================================
        // Delete - login required
        // ================================
        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _studentManager.DeleteAsync(id, LoggedInUser);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
