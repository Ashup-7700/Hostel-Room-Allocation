using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;

namespace Kemar.HRM.Repository.Interface
{
    public interface IFeeStructure
    {
        Task<ResultModel> AddOrUpdateAsync(FeeStructureRequest request);
        Task<ResultModel> GetByIdAsync(int feeStructureId);
        Task<ResultModel> GetByFilterAsync(FeeStructureFilter filter);
        Task<ResultModel> DeleteAsync(int feeStructureId, string deletedBy);

    }
}
