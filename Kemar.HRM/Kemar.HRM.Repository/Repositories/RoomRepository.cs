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
    public class RoomRepository : IRoom
    {
        private readonly HostelDbContext _context;
        private readonly IMapper _mapper;

        public RoomRepository(HostelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultModel> AddOrUpdateAsync(RoomRequest request)
        {
            try
            {
                if (request.RoomId.HasValue && request.RoomId.Value > 0)
                {
                    var existing = await _context.Rooms
                        .FirstOrDefaultAsync(r => r.RoomId == request.RoomId.Value);

                    if (existing == null)
                        return ResultModel.NotFound("Room not found");

                    _mapper.Map(request, existing);

                    existing.UpdatedAt = DateTime.UtcNow;
                    if (!string.IsNullOrWhiteSpace(request.UpdatedBy))
                        existing.UpdatedBy = request.UpdatedBy;

                    await _context.SaveChangesAsync();
                    return ResultModel.Updated(null, "Room updated successfully");
                }

                var entity = _mapper.Map<Room>(request);
                entity.CreatedAt = DateTime.UtcNow;
                entity.CreatedBy = request.CreatedBy;
                entity.IsActive = true;

                await _context.Rooms.AddAsync(entity);
                await _context.SaveChangesAsync();

                return ResultModel.Created(null, "Room created successfully");
            }
            catch (Exception ex)
            {
                return ResultModel.Failure(ResultCode.ExceptionThrown, ex.Message);
            }
        }

        public async Task<ResultModel> GetByIdAsync(int roomId)
        {
            var entity = await _context.Rooms
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RoomId == roomId);

            if (entity == null)
                return ResultModel.NotFound("Room not found");

            var dto = _mapper.Map<RoomResponse>(entity);
            return ResultModel.Success(dto, "Room fetched successfully");
        }

        public async Task<ResultModel> GetByFilterAsync(RoomFilter filter)
        {
            var query = _context.Rooms.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.RoomType))
                query = query.Where(r => r.RoomType.Contains(filter.RoomType));

            if (!string.IsNullOrWhiteSpace(filter.RoomNumber))
                query = query.Where(r => r.RoomNumber.Contains(filter.RoomNumber));

            if (filter.Floor.HasValue)
                query = query.Where(r => r.Floor == filter.Floor.Value);

            if (filter.IsActive.HasValue)
                query = query.Where(r => r.IsActive == filter.IsActive.Value);

            var list = await query.ToListAsync();

            return ResultModel.Success(
                _mapper.Map<List<RoomResponse>>(list),
                "Room list fetched successfully"
            );
        }

        public async Task<ResultModel> DeleteAsync(int roomId, string deletedBy = null)
        {
            var existing = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomId == roomId);

            if (existing == null)
                return ResultModel.NotFound("Room not found");

            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(deletedBy))
                existing.UpdatedBy = deletedBy;

            await _context.SaveChangesAsync();

            return ResultModel.Success(null, "Room deleted successfully");
        }

        public async Task<bool> ExistsByRoomNumberAsync(string roomNumber, int? excludingRoomId = null)
        {
            roomNumber = (roomNumber ?? string.Empty).Trim().ToLower();

            return await _context.Rooms
                .AnyAsync(r =>
                    r.RoomNumber.Trim().ToLower() == roomNumber &&
                    (excludingRoomId == null || r.RoomId != excludingRoomId.Value)
                );
        }
    }
}
