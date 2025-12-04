using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;

namespace Kemar.HRM.Repository.Interface
{
    public interface IPayment
    {
        Task<ResultModel> AddOrUpdateAsync(PaymentRequest request);
        Task<ResultModel> GetByIdAsync(int paymentId);
        Task<ResultModel> GetByFilterAsync(PaymentFilter filter);
        Task<ResultModel> DeleteAsync(int paymentId, string deletedBy);

    }
}
