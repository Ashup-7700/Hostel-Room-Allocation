using Kemar.HRM.Business.Interface;
using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Repository.Interface;
using System.Text.RegularExpressions;

namespace Kemar.HRM.Business
{
    public class PaymentManager : IPaymentManager
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentManager(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        // ----------------------------------------------------
        // Add or Update Payment with Business Validation
        // ----------------------------------------------------
        public async Task<ResultModel> AddOrUpdateAsync(PaymentRequest request)
        {
            // Validate StudentId
            if (request.StudentId <= 0)
                return ResultModel.Failure(message: "Invalid StudentId");

            // Validate Amount
            if (request.Amount <= 0)
                return ResultModel.Failure(message: "Payment amount must be greater than zero");

            // Validate PaymentMode
            if (string.IsNullOrWhiteSpace(request.PaymentMode))
                return ResultModel.Failure(message: "Payment mode is required");

            // Optional: Validate PaymentStatus
            var validStatuses = new[] { "Pending", "Completed", "Failed" };
            if (string.IsNullOrWhiteSpace(request.PaymentStatus) || !validStatuses.Contains(request.PaymentStatus))
                return ResultModel.Failure(message: $"PaymentStatus must be one of: {string.Join(", ", validStatuses)}");

            // Optional: prevent duplicate payment for same student/date
            var filter = new PaymentFilter { StudentId = request.StudentId };
            var existingPaymentsResult = await _paymentRepository.GetByFilterAsync(filter);
            if (existingPaymentsResult.IsSuccess && existingPaymentsResult.Data is List<object> existingList)
            {
                if (request.PaymentId == 0) // only for new payments
                {
                    var duplicate = existingList.Cast<dynamic>().FirstOrDefault(p =>
                        p.PaymentDate.Date == request.PaymentDate.Date &&
                        p.PaymentStatus == "Completed");

                    if (duplicate != null)
                        return ResultModel.Failure(message: "Payment for this student already exists for the given date");
                }
            }

            // Delegate to repository
            return await _paymentRepository.AddOrUpdateAsync(request);
        }

        // ----------------------------------------------------
        // Get Payment By ID
        // ----------------------------------------------------
        public async Task<ResultModel> GetByIdAsync(int paymentId)
        {
            if (paymentId <= 0)
                return ResultModel.Failure(message: "Invalid PaymentId");

            return await _paymentRepository.GetByIdAsync(paymentId);
        }

        // ----------------------------------------------------
        // Get Payments by Filter
        // ----------------------------------------------------
        public async Task<ResultModel> GetByFilterAsync(PaymentFilter filter)
        {
            if (filter == null)
                return ResultModel.Failure(message: "Filter is required");

            return await _paymentRepository.GetByFilterAsync(filter);
        }

        // ----------------------------------------------------
        // Delete Payment (Soft Delete) with Validation
        // ----------------------------------------------------
        public async Task<ResultModel> DeleteAsync(int paymentId, string deletedBy)
        {
            if (paymentId <= 0)
                return ResultModel.Failure(message: "Invalid PaymentId");

            if (string.IsNullOrWhiteSpace(deletedBy))
                deletedBy = "System";

            return await _paymentRepository.DeleteAsync(paymentId, deletedBy);
        }
    }
}
