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
            if (request.RoomId.HasValue && request.RoomId > 0)
            {
                var entity = await _context.Rooms
                    .FirstOrDefaultAsync(r => r.RoomId == request.RoomId.Value);

                if (entity == null)
                    return ResultModel.NotFound("Room not found");

                entity.RoomNumber = request.RoomNumber;
                entity.RoomType = request.RoomType;
                entity.Floor = request.Floor;
                entity.Capacity = request.Capacity;
                entity.CurrentOccupancy = request.CurrentOccupancy;

                _context.Entry(entity).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return ResultModel.Updated(null, "Room updated successfully");
            }

            var newEntity = _mapper.Map<Room>(request);

            await _context.Rooms.AddAsync(newEntity);
            await _context.SaveChangesAsync();

            return ResultModel.Created(newEntity, "Room created successfully");
        }

        public async Task<ResultModel> GetByIdAsync(int roomId)
        {
            var entity = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);

            if (entity == null)
                return ResultModel.NotFound("Room not found");

            return ResultModel.Success(entity, "Room fetched successfully");
        }

        public async Task<ResultModel> GetByFilterAsync(RoomFilter filter)
        {
            var query = _context.Rooms.AsQueryable();

            if (!string.IsNullOrEmpty(filter.RoomType))
                query = query.Where(r => r.RoomType == filter.RoomType);

            if (!string.IsNullOrEmpty(filter.RoomNumber))
                query = query.Where(r => r.RoomNumber.Contains(filter.RoomNumber));

            if (filter.Floor.HasValue)
                query = query.Where(r => r.Floor == filter.Floor.Value);

            if (filter.IsActive.HasValue)
                query = query.Where(r => r.IsActive == filter.IsActive.Value);

            var list = await query.ToListAsync();

            return ResultModel.Success(list, "Rooms fetched successfully");
        }

        public async Task<ResultModel> DeleteAsync(int roomId, string deletedBy)
        {
            var entity = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);

            if (entity == null)
                return ResultModel.NotFound("Room not found");

            entity.IsActive = false;

            entity.UpdatedBy = deletedBy;
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return ResultModel.Success(null, "Room deleted successfully");
        }



        public async Task<bool> UpdateCurrentOccupancyAsync(int roomId, int newOccupancy)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
                return false;

            room.CurrentOccupancy = newOccupancy;
            room.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
