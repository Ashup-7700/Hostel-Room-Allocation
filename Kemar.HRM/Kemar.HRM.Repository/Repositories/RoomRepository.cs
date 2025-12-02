using AutoMapper;
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

        public async Task<IEnumerable<RoomResponse>> GetAllAsync()
        {
            var result = await _context.Rooms
                .Include(r => r.RoomAllocations)
                           .ToListAsync();
        
           return _mapper.Map<IEnumerable<RoomResponse>>(result);
        }

        public async Task<RoomResponse?> GetByIdAsync(int id)
        {
            var result = await _context.Rooms
                .Include(r => r.RoomAllocations)
                .FirstOrDefaultAsync(r => r.RoomId == id);

            return _mapper.Map<RoomResponse?>(result);
        }

        public async Task<IEnumerable<RoomResponse>> GetAvailableRoomAsync()
        {
            var result = await _context.Rooms
                .Where(r => r.CurrentOccupancy < r.Capacity)
                .ToListAsync();

            return _mapper.Map<IEnumerable<RoomResponse>>(result);

        }

        public async Task<RoomResponse> CreateAsync(RoomRequest request)
        {

            var result = _mapper.Map<Room>(request);

            _context.Rooms.Add(result);
            await _context.SaveChangesAsync();

            return _mapper.Map<RoomResponse>(result);
        }

        public async Task<RoomResponse?> UpdateAsync(int id, RoomRequest request)
        {

            var result = await _context.Rooms.FindAsync(id);

            if(result == null)
                return null;

            _mapper.Map(request, result);
            result.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<RoomResponse?>(result);
                        
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _context.Rooms.FindAsync(id);
            if (result == null) return false;

            _context.Rooms.Remove(result);  
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> IncreaseOccupancyAsync(int roomId)
        {
            var result = await _context.Rooms.FindAsync(roomId);
            if(result == null) return false;

            result.CurrentOccupancy++;
            result.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DecreaseOccupancyAsync(int roomId)
        {
            var result = await _context.Rooms.FindAsync(roomId);
            if(result == null) return false;

            result.CurrentOccupancy--;
            result.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }


    }
}
