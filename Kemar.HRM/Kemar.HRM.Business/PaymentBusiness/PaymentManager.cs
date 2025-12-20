using Kemar.HRM.Business.Interface;
using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Repository.Interface;

namespace Kemar.HRM.Business.PaymentBusiness
{
    public class PaymentManager : IPaymentManager
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentManager(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        // ➕ ADD / UPDATE PAYMENT
        public async Task<ResultModel> AddOrUpdateAsync(PaymentRequest request)
        {
            if (request.StudentId <= 0)
                return ResultModel.Failure(ResultCode.Invalid, "Invalid StudentId");

            if (request.PaidAmount < 0)
                return ResultModel.Failure(ResultCode.Invalid, "Paid amount must be greater than or equal to zero");

            if (string.IsNullOrWhiteSpace(request.PaymentMode))
                return ResultModel.Failure(ResultCode.Invalid, "Payment mode is required");

            if (request.TotalAmount <= 0)
                return ResultModel.Failure(ResultCode.Invalid, "Total amount must be provided and greater than zero");

            // 🔹 Calculate PaymentStatus (RemainingAmount is auto-calculated in PaymentResponse)
            request.PaymentStatus = (request.TotalAmount - request.PaidAmount) <= 0 ? "Completed" : "Pending";

            // Save or update payment
            var result = await _paymentRepository.AddOrUpdateAsync(request);

            // Only set PaymentStatus in response if applicable
            if (result?.Data != null)
            {
                dynamic payment = result.Data;
                payment.PaymentStatus = (payment.TotalAmount - payment.PaidAmount) <= 0 ? "Completed" : "Pending";
                // ❌ DO NOT assign RemainingAmount (read-only)
            }

            return result;
        }

        // 🔍 GET PAYMENT BY ID
        public async Task<ResultModel> GetByIdAsync(int paymentId)
        {
            if (paymentId <= 0)
                return ResultModel.Failure(ResultCode.Invalid, "Invalid PaymentId");

            var result = await _paymentRepository.GetByIdAsync(paymentId);

            if (result?.Data != null)
            {
                dynamic payment = result.Data;
                payment.PaymentStatus = (payment.TotalAmount - payment.PaidAmount) <= 0 ? "Completed" : "Pending";
                // ❌ DO NOT assign RemainingAmount
            }

            return result;
        }

        // 🔍 GET PAYMENTS BY STUDENT ID
        public async Task<ResultModel> GetByStudentIdAsync(int studentId)
        {
            if (studentId <= 0)
                return ResultModel.Failure(ResultCode.Invalid, "Invalid StudentId");

            var result = await _paymentRepository.GetByStudentIdAsync(studentId);

            if (result?.Data is IEnumerable<dynamic> payments)
            {
                foreach (var payment in payments)
                {
                    payment.PaymentStatus = (payment.TotalAmount - payment.PaidAmount) <= 0 ? "Completed" : "Pending";
                    // ❌ DO NOT assign RemainingAmount
                }
            }

            return result;
        }

        // 📋 FILTER PAYMENTS
        public async Task<ResultModel> GetByFilterAsync(PaymentFilter filter)
        {
            if (filter == null)
                return ResultModel.Failure(ResultCode.Invalid, "Filter is required");

            var result = await _paymentRepository.GetByFilterAsync(filter);

            if (result?.Data is IEnumerable<dynamic> payments)
            {
                foreach (var payment in payments)
                {
                    payment.PaymentStatus = (payment.TotalAmount - payment.PaidAmount) <= 0 ? "Completed" : "Pending";
                    // ❌ DO NOT assign RemainingAmount
                }
            }

            return result;
        }

        // ❌ SOFT DELETE PAYMENT
        public async Task<ResultModel> DeleteAsync(int paymentId, string deletedBy)
        {
            if (paymentId <= 0)
                return ResultModel.Failure(ResultCode.Invalid, "Invalid PaymentId");

            if (string.IsNullOrWhiteSpace(deletedBy))
                deletedBy = "System";

            return await _paymentRepository.DeleteAsync(paymentId, deletedBy);
        }
    }
}
