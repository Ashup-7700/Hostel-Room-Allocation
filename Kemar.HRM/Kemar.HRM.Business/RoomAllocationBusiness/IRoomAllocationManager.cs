using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;

namespace Kemar.HRM.Business.RoomAllocationBusiness
{
    public interface IRoomAllocationManager
    {
        Task<ResultModel> AddOrUpdateAsync(RoomAllocationRequest request);
        Task<ResultModel> GetByIdAsync(int id);
        Task<ResultModel> GetByFilterAsync(RoomAllocationFilter filter);
        Task<ResultModel> DeleteAsync(int id);
    }
}
