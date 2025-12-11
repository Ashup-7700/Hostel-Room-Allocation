using Kemar.HRM.Business.UserBusiness;
using Kemar.HRM.Model.Common;
using Kemar.HRM.Model.Filter;
using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;
using Kemar.HRM.Repository.Interface;
using Microsoft.AspNetCore.Http;

namespace Kemar.HRM.Business.RoomAllocationBusiness
{
    public class RoomAllocationManager : IRoomAllocationManager
    {
        private readonly IRoomAllocation _repository;
        private readonly IRoom _roomRepository;
        private readonly IUserManager _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public RoomAllocationManager(
            IRoomAllocation repository,
            IRoom roomRepository,
            IUserManager userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _roomRepository = roomRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCurrentUsername()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == "UserId")?.Value;

            return userIdClaim != null ? int.Parse(userIdClaim) : 0;
        }

        private RoomRequest MapRoomEntityToRequestForOccupancy(Repository.Entity.Room room)
        {
            return new RoomRequest
            {
                RoomId = room.RoomId,
                CurrentOccupancy = room.CurrentOccupancy,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                Capacity = room.Capacity,
                Floor = room.Floor
            };
        }
        private async Task<bool> CheckUserExistsAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user?.Data != null;
        }
        public async Task<ResultModel> AddOrUpdateAsync(RoomAllocationRequest request)
        {
           //request.AllocatedByUserId = GetCurrentUserId();

            string username = GetCurrentUsername();

            if (request.RoomAllocationId == 0)
            {
                var roomResult = await _roomRepository.GetByIdAsync(request.RoomId);
                if (roomResult?.Data is not Repository.Entity.Room room || !room.IsActive)
                    return ResultModel.NotFound("Room not found or inactive.");

                if (room.CurrentOccupancy >= room.Capacity)
                    return ResultModel.Failure(ResultCode.NotAllowed, "Room is already full.");

                room.CurrentOccupancy += 1;
                //room.Capacity -= 1;

                var roomRequest = MapRoomEntityToRequestForOccupancy(room);
                await _roomRepository.AddOrUpdateAsync(roomRequest);
            }
            else
            {

                var existingResult = await _repository.GetByIdAsync(request.RoomAllocationId);
                if (existingResult?.Data is RoomAllocationResponse existingAlloc)
                {

                    if (request.ReleasedAt.HasValue && !existingAlloc.ReleasedAt.HasValue)
                    {
                        var roomResult = await _roomRepository.GetByIdAsync(existingAlloc.RoomId);
                        if (roomResult?.Data is Repository.Entity.Room roomEntity)
                        {
                            roomEntity.CurrentOccupancy = Math.Max(0, roomEntity.CurrentOccupancy - 1);

                            var roomRequest = MapRoomEntityToRequestForOccupancy(roomEntity);
                            await _roomRepository.AddOrUpdateAsync(roomRequest);
                        }
                    }
                }
            }

            return await _repository.AddOrUpdateAsync(request, username);
        }

        public async Task<ResultModel> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<ResultModel> GetByFilterAsync(RoomAllocationFilter filter)
        {
            return await _repository.GetByFilterAsync(filter);
        }

        public async Task<ResultModel> DeleteAsync(int id)
        {
            string username = GetCurrentUsername();

            var allocationResult = await _repository.GetByIdAsync(id);
            if (allocationResult?.Data is RoomAllocationResponse alloc)
            {
                if (alloc.IsActive)
                {

                    var roomResult = await _roomRepository.GetByIdAsync(alloc.RoomId);
                    if (roomResult?.Data is Repository.Entity.Room room)
                    {
                        room.CurrentOccupancy = Math.Max(0, room.CurrentOccupancy - 1);

                        var roomRequest = MapRoomEntityToRequestForOccupancy(room);
                        await _roomRepository.AddOrUpdateAsync(roomRequest);
                    }
                }
            }

            return await _repository.DeleteAsync(id, username);
        }
    }
}
