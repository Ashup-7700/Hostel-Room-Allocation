using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;

namespace Kemar.HRM.Repository.Interface
{
    public interface IRoom
    {
        Task<IEnumerable<RoomResponse>> GetAllAsync();
        Task<RoomResponse?> GetByIdAsync(int id);
        Task<IEnumerable<RoomResponse>> GetAvailableRoomAsync();

        Task<RoomResponse> CreateAsync(RoomRequest request);
        Task<RoomResponse?> UpdateAsync(int id, RoomRequest request);
        Task<bool> DeleteAsync(int id);

        Task<bool> IncreaseOccupancyAsync(int roomId);
        Task<bool> DecreaseOccupancyAsync(int roomId);
    }
}
