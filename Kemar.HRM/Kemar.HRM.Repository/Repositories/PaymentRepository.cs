using AutoMapper;
using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;
using Kemar.HRM.Repository.Context;
using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Kemar.HRM.Repository.Repositories
{
    public class PaymentRepository : IPayment
    {
        private readonly HostelDbContext _context;
        private readonly IMapper _mapper;

        public PaymentRepository(HostelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultModel> AddOrUpdateAsync(PaymentRequest request)
        {
            try
            {
                if (request.PaymentId.HasValue && request.PaymentId.Value > 0)
                {
                    var existing = await _context.Payments
                        .FirstOrDefaultAsync(p => p.PaymentId == request.PaymentId.Value);

                    if (existing == null)
                        return ResultModel.NotFound("Payment not found");

                    _mapper.Map(request, existing);

                    existing.UpdatedAt = DateTime.UtcNow;
                    existing.UpdatedBy = request.UpdatedBy;

                    await _context.SaveChangesAsync();

                    return ResultModel.Updated(null, "Payment updated successfully");
                }
                else
                {
                    var entity = _mapper.Map<Payment>(request);

                    entity.CreatedAt = DateTime.UtcNow;
                    entity.IsActive = request.IsActive ?? true;
                    entity.CreatedBy = request.CreatedBy;

                    _context.Payments.Add(entity);
                    await _context.SaveChangesAsync();

                    return ResultModel.Created(null, "Payment created successfully");
                }
            }
            catch (Exception ex)
            {
                return ResultModel.Failure(ResultCode.ExceptionThrown, ex.Message);
            }
        }

        public async Task<ResultModel> GetByIdAsync(int paymentId)
        {
            var entity = await _context.Payments.AsNoTracking()
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            if (entity == null)
                return ResultModel.NotFound("Payment not found");

            var dto = _mapper.Map<PaymentResponse>(entity);
            return ResultModel.Success(dto);
        }

        public async Task<ResultModel> GetByFilterAsync(PaymentFilter filter)
        {
            var query = _context.Payments.AsNoTracking().AsQueryable();

            if (filter.StudentId.HasValue)
                query = query.Where(p => p.StudentId == filter.StudentId);

            if (!string.IsNullOrWhiteSpace(filter.PaymentMethod))
                query = query.Where(p => p.PaymentMethod.Contains(filter.PaymentMethod));

            if (!string.IsNullOrWhiteSpace(filter.PaymentType))
                query = query.Where(p => p.PaymentType.Contains(filter.PaymentType));

            if (filter.FromDate.HasValue)
                query = query.Where(p => p.PaymentDate >= filter.FromDate);

            if (filter.ToDate.HasValue)
                query = query.Where(p => p.PaymentDate <= filter.ToDate);

            var items = await query.OrderByDescending(p => p.PaymentDate).ToListAsync();

            return ResultModel.Success(_mapper.Map<List<PaymentResponse>>(items));
        }

        public async Task<ResultModel> DeleteAsync(int paymentId, string deletedBy)
        {
            var existing = await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            if (existing == null)
                return ResultModel.NotFound("Payment not found");

            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.UpdatedBy = deletedBy;

            await _context.SaveChangesAsync();

            return ResultModel.Success(null, "Payment deleted successfully");

        }
    }
}
