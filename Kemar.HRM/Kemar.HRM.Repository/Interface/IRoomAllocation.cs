using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;

namespace Kemar.HRM.Repository.Interface
{
    public interface IRoomAllocation
    {
        Task<ResultModel> AddOrUpdateAsync(RoomAllocationRequest request);
        Task<ResultModel> GetByIdAsync(int allocationId);
        Task<ResultModel> GetByFilterAsync(RoomAllocationFilter filter);
        Task<ResultModel> DeleteAsync(int allocationId, string deletedBy = null);
        Task<bool> ExistsActiveAllocationAsync(int studentId, int roomId);

    }
}
