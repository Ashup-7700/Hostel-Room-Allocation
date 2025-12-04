using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;

namespace Kemar.HRM.Business.UserBusiness
{
    public interface IUserManager
    {
        Task<ResultModel> AddOrUpdateAsync(UserRequest request);
        Task<ResultModel> GetByIdAsync(int userId);
        Task<ResultModel> GetByFilterAsync(UserFilter filter);
        Task<ResultModel> DeleteAsync(int userId, string deletedBy = null);

    }
}
