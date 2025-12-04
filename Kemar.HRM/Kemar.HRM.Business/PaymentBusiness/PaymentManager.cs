using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Repository.Interface;

namespace Kemar.HRM.Business.PaymentBusiness
{
    public class PaymentManager : IPaymentManager
    {
        private readonly IPayment _repo;

        public PaymentManager(IPayment repo)
        {
            _repo = repo;
        }

        public Task<ResultModel> AddOrUpdateAsync(PaymentRequest request)
            => _repo.AddOrUpdateAsync(request);

        public Task<ResultModel> DeleteAsync(int paymentId, string deletedBy)
            => _repo.DeleteAsync(paymentId, deletedBy);

        public Task<ResultModel> GetByFilterAsync(PaymentFilter filter)
            => _repo.GetByFilterAsync(filter);

        public Task<ResultModel> GetByIdAsync(int paymentId)
            => _repo.GetByIdAsync(paymentId);

    }
}
