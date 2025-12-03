using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;

namespace Kemar.HRM.Repository.Interface
{
    public interface IUser
    {
        Task<ResultModel> AddOrUpdateAsync(UserRequest request);
        Task<ResultModel> GetByIdAsync(int userId);
        Task<ResultModel> GetByFilterAsync(UserFilter filter);
        Task<bool> ExistsByEmailAsync(string email, int? excludingUserId = null);
        Task<ResultModel> DeleteAsync(int userId, string deletedBy = null);
    }
}
