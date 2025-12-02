using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;
using Kemar.HRM.Repository.Interface;

namespace Kemar.HRM.Business.RoomBusiness
{
    public class RoomManager : IRoomManager
    {
        private readonly IRoom _repo;

        public RoomManager(IRoom repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<RoomResponse>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<RoomResponse?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        public Task<IEnumerable<RoomResponse>> GetAvailableRoomAsync()
            => _repo.GetAvailableRoomAsync();

        public Task<RoomResponse> CreateAsync(RoomRequest request)
            => _repo.CreateAsync(request);

        public Task<RoomResponse> UpdateAsync(int id, RoomRequest request)
            => _repo.UpdateAsync(id, request);

        public Task<bool> DeleteAsync(int id)
            => _repo.DeleteAsync(id);



        public async Task<bool> AllocateRoomAsync(int StudentId,int roomId)
        {
            var result = await _repo.GetByIdAsync(roomId);
            if(result == null) return false;

            if(result.CurrentOccupancy >= result.Capacity) return false;

            return await _repo.IncreaseOccupancyAsync(roomId);
        }

        public async Task<bool> FreeRoomAsync(int studentId, int roomId)
        {
            var result = await _repo.GetByIdAsync(roomId);
            if(result == null) return false;

            if(result.CurrentOccupancy <= 0)
                return false;

            return await _repo.DecreaseOccupancyAsync(roomId);
        }
    } 
}
