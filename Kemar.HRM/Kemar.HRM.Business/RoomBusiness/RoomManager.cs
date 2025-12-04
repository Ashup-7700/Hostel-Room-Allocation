using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Repository.Interface;

namespace Kemar.HRM.Business.RoomBusiness
{
    public class RoomManager : IRoomManager
    {
        private readonly IRoom _repo;

        public RoomManager(IRoom repo)
        {
            _repo = repo;
        }

        public async Task<ResultModel> AddOrUpdateAsync(RoomRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RoomNumber))
                return ResultModel.Failure(ResultCode.Invalid, "Room Number is required");

            var exists = await _repo.ExistsByRoomNumberAsync(
                request.RoomNumber,
                request.RoomId
            );

            if (exists)
                return ResultModel.Failure(ResultCode.DuplicateRecord, "Room number already exists");

            return await _repo.AddOrUpdateAsync(request);
        }

        public Task<ResultModel> DeleteAsync(int roomId, string deleteBy)
            => _repo.DeleteAsync(roomId, deleteBy);

        public Task<ResultModel> GetByFilterAsync(RoomFilter filter)
            => _repo.GetByFilterAsync(filter);

        public Task<ResultModel> GetByIdAsync(int roomId)
            => _repo.GetByIdAsync(roomId);

    }
}
