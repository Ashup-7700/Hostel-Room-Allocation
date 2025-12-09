using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Repository.Interface;

namespace Kemar.HRM.Business.RoomBusiness
{
    public class RoomManager : IRoomManager
    {
        private readonly IRoom _repository;

        public RoomManager(IRoom repository)
        {
            _repository = repository;
        }

        public async Task<ResultModel> AddOrUpdateAsync(RoomRequest request)
        {
            if (request.Capacity <= 0)
                return ResultModel.Failure(ResultCode.Invalid , "Room capacity must be greater than 0");

            var result = await _repository.AddOrUpdateAsync(request);
            return result;
        }

        public async Task<ResultModel> GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            return result;
        }

        public async Task<ResultModel> GetByFilterAsync(RoomFilter filter)
        {
            var result = await _repository.GetByFilterAsync(filter);
            return result;
        }

        public async Task<ResultModel> DeleteAsync(int id, string username)
        {
            var result = await _repository.DeleteAsync(id, username);
            return result;
        }
    }
}
