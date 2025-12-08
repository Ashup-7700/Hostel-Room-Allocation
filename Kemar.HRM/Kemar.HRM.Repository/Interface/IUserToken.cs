using Kemar.HRM.Model.Common;
using Kemar.HRM.Repository.Entity;

namespace Kemar.HRM.Repository.Interface
{
    public interface IUserToken
    {
        Task<ResultModel> AddAsync(UserToken entity);
        Task<ResultModel> GetByUserIdAsync(int userId);
        Task<ResultModel> DeleteAsync(int userTokenId);
    }
}
