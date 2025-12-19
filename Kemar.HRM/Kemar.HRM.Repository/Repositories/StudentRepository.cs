using AutoMapper;
using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;
using Kemar.HRM.Repository.Context;
using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Kemar.HRM.Repository.Repositories
{
    public class StudentRepository : IStudent
    {
        private readonly HostelDbContext _context;
        private readonly IMapper _mapper;

        public StudentRepository(HostelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultModel> AddOrUpdateAsync(StudentRequest request, string loginUser)
        {
            try
            {
                if (request.StudentId > 0)
                {
                    //var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == request.StudentId);

                                    var student = await _context.Students
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(s => s.StudentId == request.StudentId);

                    if (student == null)
                        return ResultModel.NotFound("Student not found");

                    student.Name = request.Name;
                    student.Gender = request.Gender;
                    student.Phone = request.Phone;
                    student.Email = request.Email;
                    student.Address = request.Address;
                    student.DateOfAdmission = request.DateOfAdmission;
                    student.IsActive = request.IsActive;
                    student.UpdatedAt = DateTime.UtcNow;
                    student.UpdatedBy = loginUser;

                    await _context.SaveChangesAsync();
                    return ResultModel.Updated(null, "Student updated successfully");
                }

                var newStudent = _mapper.Map<Student>(request);
                newStudent.CreatedBy = loginUser;
                newStudent.CreatedAt = DateTime.UtcNow;

                await _context.Students.AddAsync(newStudent);
                await _context.SaveChangesAsync();

                return ResultModel.Created(null, "Student created successfully");
            }
            catch (Exception ex)
            {
                return ResultModel.Failure(ResultCode.ExceptionThrown, ex.Message);
            }
        }

        public async Task<ResultModel> GetByIdAsync(int studentId)
        {
            var student = await _context.Students.AsNoTracking()
                                .FirstOrDefaultAsync(s => s.StudentId == studentId && s.IsActive);

            if (student == null)
                return ResultModel.NotFound("Student not found");

            var dto = _mapper.Map<StudentResponse>(student);
            return ResultModel.Success(dto, "Student fetched successfully");
        }

        public async Task<ResultModel> GetAllAsync()
        {
            var students = await _context.Students.IgnoreQueryFilters().AsNoTracking().ToListAsync();
            var dto = _mapper.Map<List<StudentResponse>>(students);
            return ResultModel.Success(dto, "Student list fetched successfully");
        }

        public async Task<ResultModel> DeleteAsync(int studentId, string loginUser)
        {
            var student = await _context.Students.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null)
                return ResultModel.NotFound("Student not found");

            //student.IsActive = false;
            //student.UpdatedBy = loginUser;
            //student.UpdatedAt = DateTime.UtcNow;

            _context.Students.Remove(student);

            await _context.SaveChangesAsync();
            return ResultModel.Success(null, "Student deleted successfully");
        }
    }
}
