using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;

namespace Kemar.HRM.Business.StudentBusiness
{
    public interface IStudentManager
    {
        Task<ResultModel> AddOrUpdateAsync(StudentRequest request);
        Task<ResultModel> GetByIdAsync(int studentId);
        Task<ResultModel> GetByFilterAsync(StudentFilter filter);
        Task<ResultModel> DeleteAsync(int studentId, string deletedBy = null);

    }
}
