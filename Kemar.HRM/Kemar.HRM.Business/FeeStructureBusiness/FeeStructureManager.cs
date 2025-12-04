using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Repository.Interface;

namespace Kemar.HRM.Business.FeeStructureBusiness
{
    public class FeeStructureManager : IFeeStructureManager
    {
        private readonly IFeeStructure _repo;

        public FeeStructureManager(IFeeStructure repo)
        {
            _repo = repo;
        }

        public async Task<ResultModel> AddOrUpdateAsync(FeeStructureRequest request)
        {
            if (request == null)
                return ResultModel.Failure(ResultCode.Invalid, "Invalid request");

            if (string.IsNullOrWhiteSpace(request.RoomType))
                return ResultModel.Failure(ResultCode.Invalid, "RoomType is required");

            if (request.MonthlyRent < 0)
                return ResultModel.Failure(ResultCode.Invalid, "MonthlyRent must be non-negative");

            return await _repo.AddOrUpdateAsync(request);
        }

        public Task<ResultModel> DeleteAsync(int feeStructureId, string deletedBy)
            => _repo.DeleteAsync(feeStructureId, deletedBy);

        public Task<ResultModel> GetByFilterAsync(FeeStructureFilter filter)
            => _repo.GetByFilterAsync(filter);

        public Task<ResultModel> GetByIdAsync(int feeStructureId)
            => _repo.GetByIdAsync(feeStructureId);

    }
}
