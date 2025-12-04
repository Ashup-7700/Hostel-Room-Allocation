using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Repository.Interface;

namespace Kemar.HRM.Business.UserBusiness
{
    public class UserManager : IUserManager
    {
        private readonly IUser _userRepo;

        public UserManager(IUser userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<ResultModel> AddOrUpdateAsync(UserRequest request)
        {
            if (request == null)
                return ResultModel.Failure(ResultCode.Invalid, "Invalid request");

            if (string.IsNullOrWhiteSpace(request.Email))
                return ResultModel.Failure(ResultCode.Invalid, "Email is required");


            request.Email = request.Email.Trim().ToLower();

            var exists = await _userRepo.ExistsByEmailAsync(
                request.Email,
                request.UserId
            );

            if (exists)
                return ResultModel.Failure(ResultCode.DuplicateRecord, "Email already exists");

            return await _userRepo.AddOrUpdateAsync(request);
        }

        public async Task<ResultModel> GetByIdAsync(int userId)
        {
            if (userId <= 0)
                return ResultModel.Failure(ResultCode.Invalid, "Invalid user id");

            return await _userRepo.GetByIdAsync(userId);
        }

        public async Task<ResultModel> GetByFilterAsync(UserFilter filter)
        {
            filter ??= new UserFilter();
            return await _userRepo.GetByFilterAsync(filter);
        }

        public async Task<ResultModel> DeleteAsync(int userId, string deletedBy = null)
        {
            if (userId <= 0)
                return ResultModel.Failure(ResultCode.Invalid, "Invalid user id");

            return await _userRepo.DeleteAsync(userId, deletedBy);

        }
    }
}
