using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Repository.Interface;

namespace Kemar.HRM.Business.StudentBusiness
{
    public class StudentManager : IStudentManager
    {
        private readonly IStudent _studentRepo;

        public StudentManager(IStudent studentRepo)
        {
            _studentRepo = studentRepo;
        }

        public Task<ResultModel> AddOrUpdateAsync(StudentRequest request, string loginUser)
            => _studentRepo.AddOrUpdateAsync(request, loginUser);

        public Task<ResultModel> GetByIdAsync(int studentId)
            => _studentRepo.GetByIdAsync(studentId);

        public Task<ResultModel> GetAllAsync()
            => _studentRepo.GetAllAsync();

        public Task<ResultModel> DeleteAsync(int studentId, string loginUser)
            => _studentRepo.DeleteAsync(studentId, loginUser);
    }
}
