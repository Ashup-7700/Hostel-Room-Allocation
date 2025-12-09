using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;

namespace Kemar.HRM.Business.RoomBusiness
{
    public interface IRoomManager
    {
        Task<ResultModel> AddOrUpdateAsync(RoomRequest request);
        Task<ResultModel> GetByIdAsync(int roomId);
        Task<ResultModel> GetByFilterAsync(RoomFilter filter);
        Task<ResultModel> DeleteAsync(int roomId, string username);
    }
}
