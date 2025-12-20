using AutoMapper;
using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;
using Kemar.HRM.Repository.Context;
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

        // ➕ ADD OR UPDATE PAYMENT
        public async Task<ResultModel> AddOrUpdateAsync(PaymentRequest request)
        {
            try
            {
                Payment entity;

                if (request.PaymentId > 0)
                {
                    entity = await _context.Payments
                        .FirstOrDefaultAsync(p => p.PaymentId == request.PaymentId && p.IsActive);

                    if (entity == null)
                        return ResultModel.NotFound("Payment record not found.");

                    // Update PaidAmount and PaymentMode only
                    entity.PaidAmount = request.PaidAmount;
                    entity.PaymentMode = request.PaymentMode;
                    entity.UpdatedAt = DateTime.UtcNow;
                    entity.UpdatedBy = request.UpdatedBy;

                    await _context.SaveChangesAsync();

                    return ResultModel.Updated(
                        _mapper.Map<PaymentResponse>(entity),
                        "Payment updated successfully");
                }
                else
                {
                    // Student validation
                    var studentExists = await _context.Students
                        .AnyAsync(s => s.StudentId == request.StudentId && s.IsActive);

                    if (!studentExists)
                        return ResultModel.Failure(ResultCode.Invalid, "Student does not exist or is inactive.");

                    entity = _mapper.Map<Payment>(request);
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.IsActive = true;

                    await _context.Payments.AddAsync(entity);
                    await _context.SaveChangesAsync();

                    return ResultModel.Created(
                        _mapper.Map<PaymentResponse>(entity),
                        "Payment created successfully");
                }
            }
            catch (Exception ex)
            {
                return ResultModel.Failure(
                    ResultCode.ExceptionThrown,
                    ex.InnerException?.Message ?? ex.Message);
            }
        }

        // 🔍 GET BY PAYMENT ID
        public async Task<ResultModel> GetByIdAsync(int paymentId)
        {
            try
            {
                var entity = await _context.Payments
                    .Include(p => p.Student)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.PaymentId == paymentId && p.IsActive);

                if (entity == null)
                    return ResultModel.NotFound("Payment not found.");

                return ResultModel.Success(_mapper.Map<PaymentResponse>(entity));
            }
            catch (Exception ex)
            {
                return ResultModel.Failure(
                    ResultCode.ExceptionThrown,
                    ex.InnerException?.Message ?? ex.Message);
            }
        }

        // 🔍 GET BY STUDENT ID
        public async Task<ResultModel> GetByStudentIdAsync(int studentId)
        {
            try
            {
                var entity = await _context.Payments
                    .Include(p => p.Student)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.StudentId == studentId && p.IsActive);

                if (entity == null)
                    return ResultModel.NotFound("Payment not found for student.");

                return ResultModel.Success(_mapper.Map<PaymentResponse>(entity));
            }
            catch (Exception ex)
            {
                return ResultModel.Failure(
                    ResultCode.ExceptionThrown,
                    ex.InnerException?.Message ?? ex.Message);
            }
        }

        // 📋 FILTER PAYMENTS
        public async Task<ResultModel> GetByFilterAsync(PaymentFilter filter)
        {
            try
            {
                var query = _context.Payments
                    .Include(p => p.Student)
                    .Where(p => p.IsActive)
                    .AsQueryable();

                if (filter.StudentId.HasValue)
                    query = query.Where(p => p.StudentId == filter.StudentId.Value);

                if (!string.IsNullOrWhiteSpace(filter.PaymentStatus))
                    query = query.Where(p => p.PaymentStatus == filter.PaymentStatus);

                var list = await query.AsNoTracking().ToListAsync();

                return ResultModel.Success(
                    _mapper.Map<List<PaymentResponse>>(list));
            }
            catch (Exception ex)
            {
                return ResultModel.Failure(
                    ResultCode.ExceptionThrown,
                    ex.InnerException?.Message ?? ex.Message);
            }
        }

        // ❌ SOFT DELETE
        public async Task<ResultModel> DeleteAsync(int paymentId, string deletedBy)
        {
            try
            {
                var entity = await _context.Payments
                    .FirstOrDefaultAsync(p => p.PaymentId == paymentId && p.IsActive);

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
                return ResultModel.Failure(
                    ResultCode.ExceptionThrown,
                    ex.InnerException?.Message ?? ex.Message);
            }
        }
    }

}

    //entity.PaymentStatus = entity.PaidAmount >= entity.TotalAmount? "Completed" : "Pending";



//entity.PaymentStatus = entity.PaidAmount >= entity.TotalAmount ? "Completed" : "Pending";