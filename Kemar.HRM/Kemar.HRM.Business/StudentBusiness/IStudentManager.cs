using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;

namespace Kemar.HRM.Business.StudentBusiness
{
    public interface IStudentManager
    {
        Task<IEnumerable<StudentResponse>> GetAllAsync();
        Task<StudentResponse?> GetByIdAsync(int id);
        Task<IEnumerable<StudentResponse>> GetByNameAsync(string name);
        Task<StudentResponse> CreateAsync(StudentRequest request);
        Task<StudentResponse?> UpdateAsync(int id, StudentRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
