using Kemar.HRM.API.Helpers;
using Kemar.HRM.Business.Interface;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kemar.HRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentManager _paymentManager;

        public PaymentController(IPaymentManager paymentManager)
        {
            _paymentManager = paymentManager;
        }

        // ➕ ADD OR UPDATE PAYMENT
        [HttpPost("addOrUpdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] PaymentRequest request)
        {
            if (request == null)
                return BadRequest("Request cannot be null");

            // Set current user info from claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var username = User.Claims.FirstOrDefault(c => c.Type == "username")?.Value ?? "System";

            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int parsedUserId))
            {
                request.CreatedByUserId = parsedUserId;
                request.CreatedBy = username;
            }

            var result = await _paymentManager.AddOrUpdateAsync(request);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        // 🔍 GET PAYMENT BY ID
        [HttpGet("GetById/{paymentId}")]
        public async Task<IActionResult> GetByIdAsync(int paymentId)
        {
            if (paymentId <= 0)
                return BadRequest("Invalid PaymentId");

            var result = await _paymentManager.GetByIdAsync(paymentId);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        // 📋 GET PAYMENTS BY FILTER
        [HttpPost("getByFilter")]
        public async Task<IActionResult> GetByFilterAsync([FromBody] PaymentFilter filter)
        {
            if (filter == null)
                return BadRequest("Filter cannot be null");

            var result = await _paymentManager.GetByFilterAsync(filter);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        // ❌ DELETE PAYMENT (Soft Delete)
        [HttpDelete("delete/{paymentId:int}")]
        public async Task<IActionResult> DeleteAsync(int paymentId)
        {
            if (paymentId <= 0)
                return BadRequest("Invalid PaymentId");

            var username = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "username")?.Value ?? "System";

            var result = await _paymentManager.DeleteAsync(paymentId, username);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }
    }
}
