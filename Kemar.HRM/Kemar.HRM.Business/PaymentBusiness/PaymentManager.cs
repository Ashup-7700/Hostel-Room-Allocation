using Kemar.HRM.Business.Interface;
using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;
using Kemar.HRM.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kemar.HRM.Business
{
    public class PaymentManager : IPaymentManager
    {
        private readonly IPaymentRepository _paymentRepository;
        private static readonly string[] ValidStatuses = { "Pending", "Completed", "Failed" };

        public PaymentManager(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<ResultModel> AddOrUpdateAsync(PaymentRequest request)
        {
            if (request.StudentId <= 0)
                return ResultModel.Failure(message: "Invalid StudentId");

            if (request.Amount <= 0)
                return ResultModel.Failure(message: "Payment amount must be greater than zero");

            if (string.IsNullOrWhiteSpace(request.PaymentMode))
                return ResultModel.Failure(message: "Payment mode is required");

            if (string.IsNullOrWhiteSpace(request.PaymentStatus) || !ValidStatuses.Contains(request.PaymentStatus))
                return ResultModel.Failure(message: $"PaymentStatus must be one of: {string.Join(", ", ValidStatuses)}");

            // Check for duplicate payment
            var filter = new PaymentFilter { StudentId = request.StudentId };
            var existingPaymentsResult = await _paymentRepository.GetByFilterAsync(filter);

            if (existingPaymentsResult.IsSuccess && existingPaymentsResult.Data is List<PaymentResponse> existingPayments)
            {
                if (request.PaymentId == 0) // Only for new payments
                {
                    var duplicate = existingPayments
                        .FirstOrDefault(p => p.PaymentDate.Date == request.PaymentDate.Date &&
                                             p.PaymentStatus.Equals("Completed", StringComparison.OrdinalIgnoreCase));

                    if (duplicate != null)
                        return ResultModel.Failure(message: "Payment for this student already exists for the given date");
                }
            }

            // Add or Update
            return await _paymentRepository.AddOrUpdateAsync(request);
        }

        public async Task<ResultModel> GetByIdAsync(int paymentId)
        {
            if (paymentId <= 0)
                return ResultModel.Failure(message: "Invalid PaymentId");

            return await _paymentRepository.GetByIdAsync(paymentId);
        }

        public async Task<ResultModel> GetByFilterAsync(PaymentFilter filter)
        {
            if (filter == null)
                return ResultModel.Failure(message: "Filter is required");

            return await _paymentRepository.GetByFilterAsync(filter);
        }

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
