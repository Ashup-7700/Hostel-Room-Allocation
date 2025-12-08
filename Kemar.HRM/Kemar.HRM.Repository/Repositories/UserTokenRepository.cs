using AutoMapper;
using Kemar.HRM.Model.Common;
using Kemar.HRM.Repository.Context;
using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.Interface;
using Microsoft.EntityFrameworkCore;


namespace Kemar.HRM.Repository.Repositories
{
    public class UserTokenRepository : IUserToken
    {

        private readonly HostelDbContext _context;
        private readonly IMapper _mapper;

        public UserTokenRepository(HostelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResultModel> AddAsync(UserToken userToken)
        {
            try
            {
                await _context.UserTokens.AddAsync(userToken);
                await _context.SaveChangesAsync();
                return ResultModel.Created(userToken, "Token created successfully");
            }
            catch (Exception ex)
            {
                return ResultModel.Failure(ResultCode.ExceptionThrown, ex.Message);
            }
        }

        public async Task<ResultModel> DeleteAsync(int userTokenId)
        {
             var token = await _context.UserTokens
                .FirstOrDefaultAsync(t => t.UserTokenId == userTokenId);

            if (token == null)
                return ResultModel.NotFound("Token not found");

            _context.UserTokens.Remove(token);
            await _context.SaveChangesAsync();

            return ResultModel.Success(null, "Token delete successfully");
        }

        public async Task<ResultModel> GetByUserIdAsync(int userId)
        {
            var tokens = await _context.UserTokens
                .AsNoTracking()
                .Where(t => t.UserId == userId)
                .ToListAsync();

            return ResultModel.Success(tokens, "Tokens Fetched Successfully");
        }
    }
}
