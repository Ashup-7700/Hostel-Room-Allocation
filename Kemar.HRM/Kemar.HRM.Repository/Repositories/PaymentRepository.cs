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
    public class PaymentRepository : IPaymentRepository
    {
        private readonly HostelDbContext _context;
        private readonly IMapper _mapper;

        public PaymentRepository(HostelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ----------------------------------------------------
        // Add or Update Payment
        // ----------------------------------------------------
        public async Task<ResultModel> AddOrUpdateAsync(PaymentRequest request)
        {
            try
            {
                Payment entity;

                if (request.PaymentId > 0)
                {
                    // UPDATE
                    entity = await _context.Payments
                        .FirstOrDefaultAsync(p => p.PaymentId == request.PaymentId);

                    if (entity == null)
                        return ResultModel.NotFound("Payment record not found.");

                    _mapper.Map(request, entity);
                    entity.UpdatedAt = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    return ResultModel.Updated(_mapper.Map<PaymentResponse>(entity), "Payment updated successfully");
                }
                else
                {
                    // CREATE
                    entity = _mapper.Map<Payment>(request);
                    entity.CreatedAt = DateTime.UtcNow;

                    await _context.Payments.AddAsync(entity);
                    await _context.SaveChangesAsync();

                    return ResultModel.Created(_mapper.Map<PaymentResponse>(entity), "Payment created successfully");
                }
            }
            catch (Exception ex)
            {
                return ResultModel.Failure(ResultCode.ExceptionThrown, ex.InnerException?.Message ?? ex.Message);
            }
        }

        // ----------------------------------------------------
        // Get Payment by ID
        // ----------------------------------------------------
        public async Task<ResultModel> GetByIdAsync(int paymentId)
        {
            try
            {
                var entity = await _context.Payments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

                if (entity == null)
                    return ResultModel.NotFound("Payment not found.");

                return ResultModel.Success(_mapper.Map<PaymentResponse>(entity));
            }
            catch (Exception ex)
            {
                return ResultModel.Failure(ResultCode.ExceptionThrown, ex.InnerException?.Message ?? ex.Message);
            }
        }

        // ----------------------------------------------------
        // Get Payments by Filter
        // ----------------------------------------------------
        public async Task<ResultModel> GetByFilterAsync(PaymentFilter filter)
        {
            try
            {
                var query = _context.Payments.AsQueryable();

                if (filter.StudentId.HasValue && filter.StudentId.Value > 0)
                    query = query.Where(p => p.StudentId == filter.StudentId.Value);

                if (!string.IsNullOrEmpty(filter.PaymentStatus))
                    query = query.Where(p => p.PaymentStatus == filter.PaymentStatus);

                var list = await query.AsNoTracking().ToListAsync();
                var mappedList = _mapper.Map<List<PaymentResponse>>(list);

                return ResultModel.Success(mappedList);
            }
            catch (Exception ex)
            {
                return ResultModel.Failure(ResultCode.ExceptionThrown, ex.InnerException?.Message ?? ex.Message);
            }
        }

        // ----------------------------------------------------
        // Delete Payment (Soft Delete)
        // ----------------------------------------------------
        public async Task<ResultModel> DeleteAsync(int paymentId, string deletedBy)
        {
            try
            {
                var entity = await _context.Payments
                    .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

                if (entity == null)
                    return ResultModel.NotFound("Payment not found.");

                entity.IsActive = false;
                entity.UpdatedBy = deletedBy;
                entity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ResultModel.Success(message: "Payment deleted successfully.");
            }
            catch (Exception ex)
            {
                return ResultModel.Failure(ResultCode.ExceptionThrown, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
