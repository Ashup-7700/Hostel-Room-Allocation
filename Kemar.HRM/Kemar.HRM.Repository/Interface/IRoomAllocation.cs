using Kemar.HRM.Repository.Entity;

namespace Kemar.HRM.Repository.Interface
{
    public interface IRoomAllocation
    {
        Task<IEnumerable<RoomAllocation>> GetAllocationsByStudent(int studentId);
        Task<IEnumerable<RoomAllocation>> GetAllocationsByRoom(int roomId);
        Task<bool> CheckoutAsync(int allocationId);
        Task<RoomAllocation> CreateAsync(RoomAllocation entity);
        Task<RoomAllocation?> UpdateAsync(RoomAllocation entity);
        Task<bool> DeleteAsync(int id);
        Task<RoomAllocation?> GetByIdAsync(int id);
        Task<IEnumerable<RoomAllocation>> GetAllAsync();

    }
}
