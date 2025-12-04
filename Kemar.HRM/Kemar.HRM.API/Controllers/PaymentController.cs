using Kemar.HRM.Business.PaymentBusiness;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace Kemar.HRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentManager _manager;

        public PaymentController(IPaymentManager manager)
        {
            _manager = manager;
        }

        [HttpPost("addOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync([FromBody] PaymentRequest request)
        {
            return Ok(await _manager.AddOrUpdateAsync(request));
        }

        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetByIdAsync(int paymentId)
        {
            return Ok(await _manager.GetByIdAsync(paymentId));
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetByFilterAsync([FromBody] PaymentFilter filter)
        {
            return Ok(await _manager.GetByFilterAsync(filter));
        }

        [HttpDelete("{paymentId}")]
        public async Task<IActionResult> DeleteAsync(int paymentId)
        {
            string deletedBy = "admin";
            return Ok(await _manager.DeleteAsync(paymentId, deletedBy));

        }
    }
}
