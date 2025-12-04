using Kemar.HRM.Business.FeeStructureBusiness;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace Kemar.HRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeeStructureController : ControllerBase
    {
        private readonly IFeeStructureManager _manager;

        public FeeStructureController(IFeeStructureManager manager)
        {
            _manager = manager;
        }

        [HttpPost("addOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync([FromBody] FeeStructureRequest request)
        {
            return Ok(await _manager.AddOrUpdateAsync(request));
        }

        [HttpGet("{feeStructureId}")]
        public async Task<IActionResult> GetByIdAsync(int feeStructureId)
        {
            return Ok(await _manager.GetByIdAsync(feeStructureId));
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetByFilterAsync([FromBody] FeeStructureFilter filter)
        {
            return Ok(await _manager.GetByFilterAsync(filter));
        }

        [HttpDelete("{feeStructureId}")]
        public async Task<IActionResult> DeleteAsync(int feeStructureId)
        {
            string deletedBy = "admin";
            return Ok(await _manager.DeleteAsync(feeStructureId, deletedBy));

        }
    }
}
