using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Repository.Interface;
using System.Threading.Tasks;

namespace Kemar.HRM.Business.StudentBusiness
{
    public class StudentManager : IStudentManager
    {
        private readonly IStudent _repo;

        public StudentManager(IStudent repo)
        {
            _repo = repo;
        }

        public async Task<ResultModel> AddOrUpdateAsync(StudentRequest request)
        {
            if (request == null)
                return ResultModel.Failure(ResultCode.Invalid, "Invalid request");

            if (string.IsNullOrWhiteSpace(request.Email))
                return ResultModel.Failure(ResultCode.Invalid, "Email is required");

            // Normalize email
            request.Email = request.Email.Trim().ToLower();

            var exists = await _repo.ExistsByEmailAsync(request.Email, request.StudentId);
            if (exists)
                return ResultModel.Failure(ResultCode.DuplicateRecord, "Email already exists");

            return await _repo.AddOrUpdateAsync(request);
        }

        public async Task<ResultModel> GetByIdAsync(int studentId)
        {
            if (studentId <= 0)
                return ResultModel.Failure(ResultCode.Invalid, "Invalid student id");

            return await _repo.GetByIdAsync(studentId);
        }

        public async Task<ResultModel> GetByFilterAsync(StudentFilter filter)
        {
            filter ??= new StudentFilter();
            return await _repo.GetByFilterAsync(filter);
        }

        public async Task<ResultModel> DeleteAsync(int studentId, string deletedBy = null)
        {
            if (studentId <= 0)
                return ResultModel.Failure(ResultCode.Invalid, "Invalid student id");

            var result = await _repo.DeleteAsync(studentId, deletedBy);

            // ensure data is null for delete
            result.Data = null;
            return result;
        }
    }
}
