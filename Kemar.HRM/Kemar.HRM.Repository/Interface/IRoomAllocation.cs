using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Repository.Entity;

namespace Kemar.HRM.Repository.Interface
{
    public interface IRoomAllocation
    {
        Task<ResultModel> AddOrUpdateAsync(RoomAllocationRequest request, string username);
        Task<ResultModel> GetByIdAsync(int id);
        Task<ResultModel> GetByFilterAsync(RoomAllocationFilter filter);
        Task<ResultModel> DeleteAsync(int id, string username);
        Task<RoomAllocation?> GetActiveByStudentIdAsync(int studentId);
    }
}
