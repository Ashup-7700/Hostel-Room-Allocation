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

                if (request.RoomAllocationId.HasValue && request.RoomAllocationId.Value > 0)
                {
                    // Update
                    entity = await _context.RoomAllocations
                        .FirstOrDefaultAsync(r => r.RoomAllocationId == request.RoomAllocationId.Value);
                    if (entity == null) return ResultModel.NotFound("Room allocation not found");

                    int oldRoomId = entity.RoomId;
                    int newRoomId = request.RoomId;

                    // Room changed
                    if (oldRoomId != newRoomId)
                    {
                        await DecreaseOccupancy(oldRoomId);
                        await IncreaseOccupancy(newRoomId);
                    }

                    // Student released
                    if (entity.ReleasedAt == null && request.ReleasedAt != null)
                        await DecreaseOccupancy(entity.RoomId);

                    entity.StudentId = request.StudentId;
                    entity.RoomId = request.RoomId;
                    entity.ReleasedAt = request.ReleasedAt;
                    entity.UpdatedAt = DateTime.UtcNow;
                    entity.UpdatedBy = username;

                    _context.RoomAllocations.Update(entity);
                    await _context.SaveChangesAsync();

                    var dto = _mapper.Map<RoomAllocationResponse>(entity);
                    return ResultModel.Updated(dto, "Room allocation updated successfully");
                }

                // Create
                entity = _mapper.Map<RoomAllocation>(request);
                entity.AllocatedAt = DateTime.UtcNow;
                entity.AllocatedByUserId = request.AllocatedByUserId ?? 0;
                entity.CreatedBy = username;
                entity.IsActive = true;

                await _context.RoomAllocations.AddAsync(entity);
                await _context.SaveChangesAsync();

                // Increase room occupancy
                await IncreaseOccupancy(entity.RoomId);

                var created = _mapper.Map<RoomAllocationResponse>(entity);
                return ResultModel.Created(created, "Room allocation created successfully");
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
                .Include(r => r.AllocatedBy)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RoomAllocationId == id);

            if (entity == null) return ResultModel.NotFound("Room allocation not found");

            var dto = _mapper.Map<RoomAllocationResponse>(entity);
            return ResultModel.Success(dto, "Room allocation fetched successfully");
        }

        public async Task<ResultModel> GetByFilterAsync(RoomAllocationFilter filter)
        {
            var query = _context.RoomAllocations
                .Include(r => r.Student)
                .Include(r => r.Room)
                .Include(r => r.AllocatedBy)
                .AsQueryable();

            if (filter.StudentId.HasValue) query = query.Where(q => q.StudentId == filter.StudentId.Value);
            if (filter.RoomId.HasValue) query = query.Where(q => q.RoomId == filter.RoomId.Value);
            if (filter.IsActive.HasValue) query = query.Where(q => q.IsActive == filter.IsActive.Value);

            var list = await query.ToListAsync();
            var dto = _mapper.Map<List<RoomAllocationResponse>>(list);

            return ResultModel.Success(dto, "Room allocations fetched successfully");
        }

        public async Task<ResultModel> DeleteAsync(int id, string username)
        {
            var entity = await _context.RoomAllocations.FirstOrDefaultAsync(r => r.RoomAllocationId == id);
            if (entity == null) return ResultModel.NotFound("Room allocation not found");

            if (entity.ReleasedAt == null)
                await DecreaseOccupancy(entity.RoomId);

            entity.IsActive = false;
            entity.ReleasedAt = DateTime.UtcNow;
            entity.UpdatedBy = username;
            entity.UpdatedAt = DateTime.UtcNow;

            _context.RoomAllocations.Update(entity);
            await _context.SaveChangesAsync();

            return ResultModel.Success(null, "Room allocation deleted successfully");
        }

        public async Task<RoomAllocation?> GetActiveByStudentIdAsync(int studentId)
        {
            return await _context.RoomAllocations
                .Where(r => r.StudentId == studentId && r.IsActive && r.ReleasedAt == null)
                .FirstOrDefaultAsync();
        }

        private async Task IncreaseOccupancy(int roomId)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);
            if (room != null)
            {
                room.CurrentOccupancy += 1;
                room.UpdatedAt = DateTime.UtcNow;
                _context.Rooms.Update(room);
                await _context.SaveChangesAsync();
            }
        }

        private async Task DecreaseOccupancy(int roomId)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);
            if (room != null && room.CurrentOccupancy > 0)
            {
                room.CurrentOccupancy -= 1;
                room.UpdatedAt = DateTime.UtcNow;
                _context.Rooms.Update(room);
                await _context.SaveChangesAsync();
            }
        }
    }
}
