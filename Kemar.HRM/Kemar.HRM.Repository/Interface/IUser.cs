using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Common;

namespace Kemar.HRM.Repository.Interface
{
        public interface IUser
        {
            Task<ResultModel> AddOrUpdateAsync(UserRequest request);
            Task<User?> AuthenticateAsync(string username, string password);
            Task<ResultModel> GetByIdAsync(int userId);
            Task<ResultModel> GetByFilterAsync(UserFilter filter);
            Task<ResultModel> DeleteAsync(int userId);
        }

    }
