using AutoMapper;
using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;
using Kemar.HRM.Repository.Context;
using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Kemar.HRM.Repository.Repositories
{
    public class UserRepository : IUser
    {
        private readonly HostelDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(HostelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultModel> AddOrUpdateAsync(UserRequest request)
        {
            try
            {
                if (request.UserId > 0)
                {
                    var existing = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId);
                    if (existing == null)
                        return ResultModel.NotFound("User not found");

                    existing.Username = request.Username;        
                    existing.FullName = request.FullName;
                    existing.Role = request.Role;
                    existing.IsActive = request.IsActive ?? existing.IsActive;

                    if (!string.IsNullOrWhiteSpace(request.Password))
                        existing.Password = HashPassword(request.Password);

                    existing.UpdatedAt = DateTime.UtcNow;
                    existing.UpdatedBy = "admin";

                    await _context.SaveChangesAsync();
                    return ResultModel.Updated(null, "User updated successfully");
                }

                var user = _mapper.Map<User>(request);
                user.Password = HashPassword(request.Password);
                user.CreatedAt = DateTime.UtcNow;
                user.CreatedBy = "admin";
                user.UpdatedAt = null;
                user.UpdatedBy = null;
                user.IsActive = request.IsActive ?? true;

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return ResultModel.Created(null, "User created successfully");
            }
            catch (Exception ex)
            {
                return ResultModel.Failure(ResultCode.ExceptionThrown, ex.Message);
            }
        }

        public async Task<ResultModel> GetByFilterAsync(UserFilter filter)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.FullName))
                query = query.Where(u => u.FullName.Contains(filter.FullName));

            if (!string.IsNullOrWhiteSpace(filter.Role))
                query = query.Where(u => u.Role == filter.Role);

            if (filter.IsActive.HasValue)
                query = query.Where(u => u.IsActive == filter.IsActive.Value);

            var list = await query.AsNoTracking().ToListAsync();

            var dto = _mapper.Map<List<UserResponse>>(list);

            return ResultModel.Success(dto, "Filtered user list fetched successfully");
        }

        public async Task<ResultModel> GetByIdAsync(int userId)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return ResultModel.NotFound("User not found");

            var dto = _mapper.Map<UserResponse>(user);
            return ResultModel.Success(dto, "User fetched successfully");
        }

        public async Task<ResultModel> GetAllAsync()
        {
            var list = await _context.Users.AsNoTracking().ToListAsync();
            var dto = _mapper.Map<List<UserResponse>>(list);
            return ResultModel.Success(dto, "User list fetched successfully");
        }

        public async Task<ResultModel> DeleteAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return ResultModel.NotFound("User not found");

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = "admin";

            await _context.SaveChangesAsync();
            return ResultModel.Success(null, "User deleted successfully");
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var hashed = HashPassword(password);
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == hashed && u.IsActive);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

    }
}
