using AutoMapper;
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

        public async Task<IEnumerable<StudentResponse>> GetAllAsync()
        {
            var data = await _context.Students
                .Include(s => s.Payments)
                .Include(s => s.RoomAllocations)
                .ToListAsync();

            return _mapper.Map<IEnumerable<StudentResponse>>(data);
        }

        public async Task<StudentResponse?> GetByIdAsync(int id)
        {
            var entity = await _context.Students
                .Include(s => s.Payments)
                .Include(s => s.RoomAllocations)
                .FirstOrDefaultAsync(x => x.StudentId == id);

            return _mapper.Map<StudentResponse?>(entity);
        }

        public async Task<IEnumerable<StudentResponse>> GetByNameAsync(string name)
        {
            var data = await _context.Students
                .Where(x => x.Name.Contains(name))
                .ToListAsync();

            return _mapper.Map<IEnumerable<StudentResponse>>(data);
        }

        public async Task<StudentResponse> CreateAsync(StudentRequest request)
        {
            var entity = _mapper.Map<Student>(request);

            _context.Students.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<StudentResponse>(entity);
        }

        public async Task<StudentResponse?> UpdateAsync(int id, StudentRequest request)
        {
            var entity = await _context.Students.FindAsync(id);
            if (entity == null) return null;

            _mapper.Map(request, entity);

            await _context.SaveChangesAsync();

            return _mapper.Map<StudentResponse>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Students.FindAsync(id);
            if (entity == null) return false;

            _context.Students.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
