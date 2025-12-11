using AutoMapper;
using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Repository.Context;
using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Kemar.HRM.Repository.Repositories
{
    public class RoomAllocationRepository : IRoomAllocation
    {
        private readonly HostelDbContext _context;
        private readonly IMapper _mapper;

        public RoomAllocationRepository(HostelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultModel> AddOrUpdateAsync(RoomAllocationRequest request, string username)
        {
            try
            {
                RoomAllocation entity;

                if (request.RoomAllocationId == 0)
                {
                    entity = _mapper.Map<RoomAllocation>(request);
                    await _context.RoomAllocations.AddAsync(entity);
                }
                else
                {
                    entity = await _context.RoomAllocations
                        .FirstOrDefaultAsync(x => x.RoomAllocationId == request.RoomAllocationId);

                    if (entity == null)
                        return ResultModel.NotFound("Allocation not found");

                    _mapper.Map(request, entity);
                   
                }

                await _context.SaveChangesAsync();
                    
                return request.RoomAllocationId == 0
                    ? ResultModel.Created(entity, "Created successfully")
                    : ResultModel.Updated(entity, "Updated successfully");
            }
            catch (Exception ex)
            {
                return ResultModel.Failure(ResultCode.ExceptionThrown, ex.Message);
            }
        }

        public async Task<ResultModel> GetByIdAsync(int id)
        {
            var entity = await _context.RoomAllocations
                .Include(r => r.Student)
                .Include(r => r.Room)
                .Include(r => r.AllocatedByUser)
                .FirstOrDefaultAsync(r => r.RoomAllocationId == id);

            if (entity == null)
                return ResultModel.NotFound();

            return ResultModel.Success(entity);
        }

        public async Task<ResultModel> GetByFilterAsync(RoomAllocationFilter filter)
        {
            var query = _context.RoomAllocations
                .Include(r => r.Student)
                .Include(r => r.Room)
                .Include(r => r.AllocatedByUser)
                .AsQueryable();

            if (filter.StudentId.HasValue)
                query = query.Where(r => r.StudentId == filter.StudentId.Value);

            if (filter.RoomId.HasValue)
                query = query.Where(r => r.RoomId == filter.RoomId.Value);

            if (filter.IsActive.HasValue)
                query = query.Where(r => r.IsActive == filter.IsActive.Value);

            var list = await query.ToListAsync();
            return ResultModel.Success(list);
        }

        public async Task<ResultModel> DeleteAsync(int id, string username)
        {
            var entity = await _context.RoomAllocations
                .FirstOrDefaultAsync(r => r.RoomAllocationId == id);

            if (entity == null)
                return ResultModel.NotFound();

            _context.RoomAllocations.Remove(entity);
            await _context.SaveChangesAsync();

            return ResultModel.Success(message: "Room allocation deleted successfully");
        }

        public async Task<RoomAllocation?> GetActiveByStudentIdAsync(int studentId)
        {
            return await _context.RoomAllocations
                .FirstOrDefaultAsync(r => r.StudentId == studentId && r.IsActive);
        }
    }
}
