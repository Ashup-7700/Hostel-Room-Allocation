using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;

namespace Kemar.HRM.Business.Interface
{
    public interface IPaymentManager
    {
        // ➕ Create or update payment (partial / full)
        Task<ResultModel> AddOrUpdateAsync(PaymentRequest request);

        // 🔍 Get payment by PaymentId
        Task<ResultModel> GetByIdAsync(int paymentId);

        // 🔍 Get payment by StudentId (used at login & room allocation)
        Task<ResultModel> GetByStudentIdAsync(int studentId);

        // 📋 Filter payments (Pending / Completed / Date wise)
        Task<ResultModel> GetByFilterAsync(PaymentFilter filter);

        // ❌ Soft delete payment
        Task<ResultModel> DeleteAsync(int paymentId, string deletedBy);
    }
}
