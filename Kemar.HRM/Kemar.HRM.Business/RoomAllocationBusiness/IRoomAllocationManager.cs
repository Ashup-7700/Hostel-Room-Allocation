using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;

namespace Kemar.HRM.Business.RoomAllocationBusiness
{
    public interface IRoomAllocationManager
    {
        Task<RoomAllocationResponse> AllocateRoomAsync(RoomAllocationRequest request);
        Task<RoomAllocationResponse> FreeRoomAsync(int allocationId);

        Task<IEnumerable<RoomAllocationResponse>> GetByStudentAsync(int studentId);
        Task<IEnumerable<RoomAllocationResponse>> GetByRoomAsync(int roomId);
        Task<RoomAllocationResponse?> GetByIdAsync(int id);
        Task<IEnumerable<RoomAllocationResponse>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}
