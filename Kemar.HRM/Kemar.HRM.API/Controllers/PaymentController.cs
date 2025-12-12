using Kemar.HRM.API.Helpers;
using Kemar.HRM.Business.Interface;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [HttpPost("addOrUpdate")]
        [Authorize]
        public async Task<IActionResult> AddOrUpdate([FromBody] PaymentRequest request)
        {
            // Try to get the user ID from claims
            int? currentUserId = null;

            // Look for "userId" exactly as in JWT payload
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int parsedUserId))
            {
                currentUserId = parsedUserId;
                request.CreatedByUserId = currentUserId; // assign real user ID
            }

            var result = await _paymentManager.AddOrUpdateAsync(request);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }



        [HttpGet("GetById/{paymentId}")]
        public async Task<IActionResult> GetByIdAsync(int paymentId)
        {
            var result = await _paymentManager.GetByIdAsync(paymentId);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [HttpPost("getByFilter")]
        public async Task<IActionResult> GetByFilterAsync([FromBody] PaymentFilter filter)
        {
            if (filter == null)
                return BadRequest("Filter cannot be null");

            var result = await _paymentManager.GetByFilterAsync(filter);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }

        [HttpDelete("delete/{paymentId:int}")]
        public async Task<IActionResult> DeleteAsync(int paymentId)
        {
            var username = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "username")?.Value ?? "System";

            var result = await _paymentManager.DeleteAsync(paymentId, username);
            return CommonHelper.ReturnActionResultByStatus(result, this);
        }
    }

}


