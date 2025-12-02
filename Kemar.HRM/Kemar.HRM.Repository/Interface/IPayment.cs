using Kemar.HRM.Repository.Entity;

namespace Kemar.HRM.Repository.Interface
{
    public interface IPayment
    {
        Task<IEnumerable<Payment>> GetPaymentByStudent(int studentId);
        Task<double> GetTotalPaidAmountAsync(int studentId);
        Task<Payment> CreateAsync(Payment entity);
        Task<Payment?> UpdateAsync(Payment entity);
        Task<bool> DeleteAsync(int id);
        Task<Payment?> GetByIdAsync(int id);
        Task<IEnumerable<Payment>> GetAllAsync();

    }
}
