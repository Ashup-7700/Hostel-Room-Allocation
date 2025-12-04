using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;

namespace Kemar.HRM.Repository.Interface
{
    public interface IStudent
    {
        Task<ResultModel> AddOrUpdateAsync(StudentRequest request);
        Task<ResultModel> GetByIdAsync(int studentId);
        Task<ResultModel> GetByFilterAsync(StudentFilter filter);
        Task<bool> ExistsByEmailAsync(string email, int? excludingStudentId = null);
        Task<ResultModel> DeleteAsync(int studentId, string deletedBy = null);

    }
}
