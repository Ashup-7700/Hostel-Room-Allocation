using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Repository.Interface;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Kemar.HRM.Business.RoomAllocationBusiness
{
    public class RoomAllocationManager : IRoomAllocationManager
    {
        private readonly IRoomAllocation _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoomAllocationManager(IRoomAllocation repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCurrentUser() => _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";

        private int GetCurrentUserId()
        {
            var claim = _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == "userId" || c.Type == ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        public async Task<ResultModel> AddOrUpdateAsync(RoomAllocationRequest request)
        {
            request.IsActive = true;
            request.AllocatedByUserId = GetCurrentUserId();
            return await _repository.AddOrUpdateAsync(request, GetCurrentUser());
        }

        public async Task<ResultModel> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<ResultModel> GetByFilterAsync(RoomAllocationFilter filter) => await _repository.GetByFilterAsync(filter);

        public async Task<ResultModel> DeleteAsync(int id) => await _repository.DeleteAsync(id, GetCurrentUser());
    }
}
