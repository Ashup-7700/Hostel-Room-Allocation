using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Repository.Interface;

namespace Kemar.HRM.Business.RoomAllocationBusiness
{
    public class RoomAllocationManager : IRoomAllocationManager
    {
        private readonly IRoomAllocation _repo;

        public RoomAllocationManager(IRoomAllocation repo)
        {
            _repo = repo;
        }

        public async Task<ResultModel> AddOrUpdateAsync(RoomAllocationRequest request)
        {
            if (request == null)
                return ResultModel.Failure(ResultCode.Invalid, "Invalid request");

            if (request.StudentId <= 0)
                return ResultModel.Failure(ResultCode.Invalid, "StudentId is required");

            if (request.RoomId <= 0)
                return ResultModel.Failure(ResultCode.Invalid, "RoomId is required");

            if (request.AllocatedAt == default)
                return ResultModel.Failure(ResultCode.Invalid, "AllocatedAt is required");

            var exists = await _repo.ExistsActiveAllocationAsync(request.StudentId, request.RoomId);
            if (exists && (!request.RoomAllocationId.HasValue || request.RoomAllocationId.Value == 0))
                return ResultModel.Failure(ResultCode.DuplicateRecord, "Active allocation already exists for this student in this room");

            return await _repo.AddOrUpdateAsync(request);
        }

        public async Task<ResultModel> GetByIdAsync(int allocationId)
        {
            if (allocationId <= 0)
                return ResultModel.Failure(ResultCode.Invalid, "Invalid allocation id");

            return await _repo.GetByIdAsync(allocationId);
        }

        public async Task<ResultModel> GetByFilterAsync(RoomAllocationFilter filter)
        {
            filter ??= new RoomAllocationFilter();
            return await _repo.GetByFilterAsync(filter);
        }

        public async Task<ResultModel> DeleteAsync(int allocationId, string deletedBy = null)
        {
            if (allocationId <= 0)
                return ResultModel.Failure(ResultCode.Invalid, "Invalid allocation id");

            return await _repo.DeleteAsync(allocationId, deletedBy);

        }
    }
}
