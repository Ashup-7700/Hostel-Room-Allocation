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
    public class FeeStructureRepository : IFeeStructure
    {
        private readonly HostelDbContext _context;
        private readonly IMapper _mapper;

        public FeeStructureRepository(HostelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultModel> AddOrUpdateAsync(FeeStructureRequest request)
        {
            try
            {
                if (request.FeeStructureId > 0)
                {
                    var existing = await _context.FeeStructures
                        .FirstOrDefaultAsync(f => f.FeeStructureId == request.FeeStructureId);

                    if (existing == null)
                        return ResultModel.NotFound("Fee Structure not found");

                    _mapper.Map(request, existing);

                    existing.UpdatedAt = DateTime.UtcNow;
                    if (!string.IsNullOrWhiteSpace(request.UpdatedBy))
                        existing.UpdatedBy = request.UpdatedBy;

                    await _context.SaveChangesAsync();
                    return ResultModel.Updated(null, "Fee Structure updated successfully");
                }

                var entity = _mapper.Map<FeeStructure>(request);
                entity.CreatedAt = DateTime.UtcNow;
                entity.CreatedBy = request.CreatedBy;
                entity.IsActive = true;

                await _context.FeeStructures.AddAsync(entity);
                await _context.SaveChangesAsync();

                return ResultModel.Created(null, "Fee Structure created successfully");
            }
            catch (Exception ex)
            {
                return ResultModel.Failure(ResultCode.ExceptionThrown, ex.Message);
            }
        }

        public async Task<ResultModel> GetByIdAsync(int feeStructureId)
        {
            var entity = await _context.FeeStructures
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.FeeStructureId == feeStructureId);

            if (entity == null)
                return ResultModel.NotFound("Fee Structure not found");

            var dto = _mapper.Map<FeeStructureResponse>(entity);
            return ResultModel.Success(dto, "Fee Structure fetched successfully");
        }

        public async Task<ResultModel> GetByFilterAsync(FeeStructureFilter filter)
        {
            var query = _context.FeeStructures
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.RoomType))
                query = query.Where(f => f.RoomType.Contains(filter.RoomType));

            if (filter.IsActive.HasValue)
                query = query.Where(f => f.IsActive == filter.IsActive.Value);

            var list = await query.ToListAsync();

            return ResultModel.Success(
                _mapper.Map<List<FeeStructureResponse>>(list),
                "Fee Structure list fetched successfully"
            );
        }

        public async Task<ResultModel> DeleteAsync(int feeStructureId, string deletedBy = null)
        {
            var existing = await _context.FeeStructures
                .FirstOrDefaultAsync(f => f.FeeStructureId == feeStructureId);

            if (existing == null)
                return ResultModel.NotFound("Fee Structure not found");

            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(deletedBy))
                existing.UpdatedBy = deletedBy;

            await _context.SaveChangesAsync();

            return ResultModel.Success(null, "Fee Structure deleted successfully");

        }
    }
}
