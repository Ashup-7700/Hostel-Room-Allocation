using Kemar.HRM.Repository.Context;
using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Kemar.HRM.Repository.Repositories
{
    public class PaymentRepository : IPayment
    {
        private readonly HostelDbContext _context;

        public PaymentRepository(HostelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetPaymentByStudent(int studentId)
        {
            return await _context.Payments
                                 .Where(p => p.StudentId == studentId)
                                 .OrderByDescending(p => p.PaymentDate)
                                 .ToListAsync();
        }

        public async Task<double> GetTotalPaidAmountAsync(int studentId)
        {
            return (double)await _context.Payments
                                 .Where(p => p.StudentId == studentId)
                                 .SumAsync(p => p.Amount);
        }

        public async Task<Payment> CreateAsync(Payment entity)
        {
            _context.Payments.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Payment?> UpdateAsync(Payment entity)
        {
            var existing = await _context.Payments.FindAsync(entity.PaymentId);

            if (existing == null)
                return null;

            existing.Amount = entity.Amount;
            existing.PaymentMethod = entity.PaymentMethod;
            existing.PaymentType = entity.PaymentType;
            existing.PaymentDate = entity.PaymentDate;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.UpdatedBy = entity.UpdatedBy;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var data = await _context.Payments.FindAsync(id);
            if (data == null)
                return false;

            _context.Payments.Remove(data);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments
                                 .Include(p => p.Student)
                                 .FirstOrDefaultAsync(p => p.PaymentId == id);
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments
                                 .Include(p => p.Student)
                                 .OrderByDescending(p => p.PaymentDate)
                                 .ToListAsync();
        }
    }
}
