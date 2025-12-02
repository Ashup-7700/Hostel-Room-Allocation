using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;

namespace Kemar.HRM.Business.RoomBusiness
{
    public interface IRoomManager
    {
        Task<IEnumerable<RoomResponse>> GetAllAsync();
        Task<RoomResponse?> GetByIdAsync(int id);
        Task<IEnumerable<RoomResponse>> GetAvailableRoomAsync();

        Task<RoomResponse> CreateAsync(RoomRequest request);
        Task<RoomResponse?> UpdateAsync(int id, RoomRequest request);
        Task<bool> DeleteAsync(int id);

        //Task<bool> AllocateRoomAsync(int studentId, int roomId);
        //Task<bool> FreeRoomAsync(int studentId, int roomId);
    }
}
