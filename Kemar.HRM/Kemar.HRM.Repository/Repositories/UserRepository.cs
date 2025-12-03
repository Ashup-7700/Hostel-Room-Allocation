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
                
                if (request.UserId.HasValue && request.UserId.Value > 0)
                {
                    var existing = await _context.Users
                        .FirstOrDefaultAsync(u => u.UserId == request.UserId.Value);

                    if (existing == null)
                        return ResultModel.NotFound("User not found");

              
                    _mapper.Map(request, existing);

                    existing.UpdatedAt = DateTime.UtcNow;
                    if (!string.IsNullOrWhiteSpace(request.UpdatedBy))
                        existing.UpdatedBy = request.UpdatedBy;

                    await _context.SaveChangesAsync();

            
                    return ResultModel.Updated(null, "User updated successfully");
                }
                else
                {
                    var entity = _mapper.Map<User>(request);
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.IsActive = true;
                    entity.CreatedBy = request.CreatedBy;

                    _context.Users.Add(entity);
                    await _context.SaveChangesAsync();

    
                    return ResultModel.Created(null, "User created successfully");
                }
            }
            catch (Exception ex)
            {
                return ResultModel.Failure(ResultCode.ExceptionThrown, ex.Message);
            }
        }

        public async Task<ResultModel> GetByIdAsync(int userId)
        {
            var entity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (entity == null)
                return ResultModel.NotFound("User not found");

            return ResultModel.Success(_mapper.Map<UserResponse>(entity), "User data fetched successfully");
        }

        public async Task<ResultModel> GetByFilterAsync(UserFilter filter)
        {
            var query = _context.Users.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.FullName))
                query = query.Where(u => u.FullName.Contains(filter.FullName));

            if (!string.IsNullOrWhiteSpace(filter.Email))
                query = query.Where(u => u.Email.Contains(filter.Email));

            if (!string.IsNullOrWhiteSpace(filter.Role))
                query = query.Where(u => u.Role == filter.Role);

            if (filter.IsActive.HasValue)
                query = query.Where(u => u.IsActive == filter.IsActive.Value);


            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                if (filter.SortBy.Equals("UserId", StringComparison.OrdinalIgnoreCase))
                    query = filter.SortDesc ? query.OrderByDescending(u => u.UserId) : query.OrderBy(u => u.UserId);
                else if (filter.SortBy.Equals("FullName", StringComparison.OrdinalIgnoreCase))
                    query = filter.SortDesc ? query.OrderByDescending(u => u.FullName) : query.OrderBy(u => u.FullName);
      
            }
            else
            {
                query = query.OrderBy(u => u.UserId);
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
                Items = _mapper.Map<List<UserResponse>>(items)
            };

            return ResultModel.Success(payload, "User list fetched successfully");
        }

        public async Task<bool> ExistsByEmailAsync(string email, int? excludingUserId = null)
        {
            email = (email ?? string.Empty).Trim().ToLower();

            return await _context.Users
                .AnyAsync(u =>
                    u.Email.Trim().ToLower() == email &&
                    (excludingUserId == null || u.UserId != excludingUserId.Value)
                );
        }

        public async Task<ResultModel> DeleteAsync(int userId, string deletedBy = null)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (existing == null)
                return ResultModel.NotFound("User not found");

            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;
            if (!string.IsNullOrWhiteSpace(deletedBy))
                existing.UpdatedBy = deletedBy;

            await _context.SaveChangesAsync();

            return ResultModel.Success(null, "User deleted successfully");
        }
    }
}
