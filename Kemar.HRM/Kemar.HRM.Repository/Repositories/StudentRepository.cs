using AutoMapper;
using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;
using Kemar.HRM.Repository.Context;
using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    // UPDATE
                    var existing = await _context.Students
                        .FirstOrDefaultAsync(s => s.StudentId == request.StudentId.Value);

                    if (existing == null)
                        return ResultModel.NotFound("Student not found");

                    _mapper.Map(request, existing);

                    existing.UpdatedAt = DateTime.UtcNow;
                    if (!string.IsNullOrWhiteSpace(request.UpdatedBy))
                        existing.UpdatedBy = request.UpdatedBy;

                    await _context.SaveChangesAsync();

                    // Return Updated code without data (per Option A behavior)
                    return ResultModel.Updated(null, "Student updated successfully");
                }
                else
                {
                    // CREATE
                    var entity = _mapper.Map<Student>(request);
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.IsActive = request.IsActive ?? true;
                    entity.CreatedBy = request.CreatedBy;

                    _context.Students.Add(entity);
                    await _context.SaveChangesAsync();

                    return ResultModel.Created(null, "Student created successfully");
                }
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
            var query = _context.Students.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(s => s.Name.Contains(filter.Name));

            if (!string.IsNullOrWhiteSpace(filter.Email))
                query = query.Where(s => s.Email.Contains(filter.Email));

            if (filter.IsActive.HasValue)
                query = query.Where(s => s.IsActive == filter.IsActive.Value);

            // Sorting
            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                if (filter.SortBy.Equals("StudentId", StringComparison.OrdinalIgnoreCase))
                    query = filter.SortDesc ? query.OrderByDescending(s => s.StudentId) : query.OrderBy(s => s.StudentId);
                else if (filter.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    query = filter.SortDesc ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name);
            }
            else
            {
                query = query.OrderBy(s => s.StudentId);
            }

            var total = await query.CountAsync();
            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var payload = new
            {
                Total = total,
                Page = filter.Page,
                PageSize = filter.PageSize,
                Items = _mapper.Map<List<StudentResponse>>(items)
            };

            return ResultModel.Success(payload, "Student list fetched successfully");
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
            var existing = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (existing == null)
                return ResultModel.NotFound("Student not found");

            // Soft delete
            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;
            if (!string.IsNullOrWhiteSpace(deletedBy))
                existing.UpdatedBy = deletedBy;

            await _context.SaveChangesAsync();

            return ResultModel.Success(null, "Student deleted successfully");
        }
    }
}
