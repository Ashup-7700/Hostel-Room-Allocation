using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;
using Kemar.HRM.Repository.Interface;

namespace Kemar.HRM.Business.StudentBusiness
{
    public class StudentManager : IStudentManager
    {
        private readonly IStudent _repo;

        public StudentManager(IStudent repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<StudentResponse>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();

            foreach (var s in data)
            {
                s.Name = s.Name?.ToUpper();
            }

            return data;
        }

        public async Task<StudentResponse?> GetByIdAsync(int id)
        {
            var data = await _repo.GetByIdAsync(id);

            if (data != null)
                Console.WriteLine("Student fetched successfully");

            return data;
        }

        public async Task<IEnumerable<StudentResponse>> GetByNameAsync(string name)
        {
            return await _repo.GetByNameAsync(name);
        }

        public async Task<StudentResponse> CreateAsync(StudentRequest request)
        {
            return await _repo.CreateAsync(request);
        }

        public async Task<StudentResponse?> UpdateAsync(int id, StudentRequest request)
        {
            return await _repo.UpdateAsync(id, request);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}