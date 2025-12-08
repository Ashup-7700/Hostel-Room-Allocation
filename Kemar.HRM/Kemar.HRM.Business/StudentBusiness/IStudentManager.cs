using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Request;

namespace Kemar.HRM.Business.StudentBusiness
{
    public interface IStudentManager
    {
        Task<ResultModel> AddOrUpdateAsync(StudentRequest request, string loginUser);
        Task<ResultModel> GetByIdAsync(int studentId);
        Task<ResultModel> GetAllAsync();
        Task<ResultModel> DeleteAsync(int studentId, string loginUser);
    }
}
