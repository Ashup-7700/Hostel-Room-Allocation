using AutoMapper;
using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
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

        public async Task<ResultModel> AddOrUpdateAsync(StudentRequest request)
        {
            try
            {
                if (request.StudentId.HasValue && request.StudentId.Value > 0)
                {
                    var existing = await _context.Students
                        .FirstOrDefaultAsync(s => s.StudentId == request.StudentId.Value);

                    if (existing == null)
                        return ResultModel.NotFound("Student not found");

                    _mapper.Map(request, existing);

                    existing.UpdatedAt = DateTime.UtcNow;
                    if (!string.IsNullOrWhiteSpace(request.UpdatedBy))
                        existing.UpdatedBy = request.UpdatedBy;

                    await _context.SaveChangesAsync();
                    return ResultModel.Updated(null, "Student updated successfully");
                }

                var entity = _mapper.Map<Student>(request);
                entity.CreatedAt = DateTime.UtcNow;
                entity.CreatedBy = request.CreatedBy;
                entity.IsActive = request.IsActive ?? true;

                await _context.Students.AddAsync(entity);
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
            var entity = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (entity == null)
                return ResultModel.NotFound("Student not found");

            var dto = _mapper.Map<StudentResponse>(entity);
            return ResultModel.Success(dto, "Student data fetched successfully");
        }

        public async Task<ResultModel> GetByFilterAsync(StudentFilter filter)
        {
            var query = _context.Students
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(s => s.Name.Contains(filter.Name));

            if (!string.IsNullOrWhiteSpace(filter.Email))
                query = query.Where(s => s.Email.Contains(filter.Email));

            if (filter.IsActive.HasValue)
                query = query.Where(s => s.IsActive == filter.IsActive.Value);

            var list = await query.ToListAsync();

            return ResultModel.Success(
                _mapper.Map<List<StudentResponse>>(list),
                "Student list fetched successfully"
            );
        }

        public async Task<bool> ExistsByEmailAsync(string email, int? excludingStudentId = null)
        {
            email = (email ?? string.Empty).Trim().ToLower();

            return await _context.Students
                .AnyAsync(s =>
                    s.Email.Trim().ToLower() == email &&
                    (excludingStudentId == null || s.StudentId != excludingStudentId.Value)
                );
        }

        public async Task<ResultModel> DeleteAsync(int studentId, string deletedBy = null)
        {
            var existing = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (existing == null)
                return ResultModel.NotFound("Student not found");

            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(deletedBy))
                existing.UpdatedBy = deletedBy;

            await _context.SaveChangesAsync();

            return ResultModel.Success(null, "Student deleted successfully");

        }
    }
}
