using Kemar.HRM.Model.Request;
using Kemar.HRM.Model.Response;
using Kemar.HRM.Repository.Interface;
using Kemar.HRM.Repository.Entity;

namespace Kemar.HRM.Business.RoomAllocationBusiness
{
    public class RoomAllocationManager : IRoomAllocationManager
    {
        private readonly IRoomAllocation _allocationRepo;
        private readonly IStudent _studentRepo;
        private readonly IRoom _roomRepo;

        public RoomAllocationManager(IRoomAllocation allocationRepo, IStudent studentRepo, IRoom roomRepo)
        {
            _allocationRepo = allocationRepo;
            _studentRepo = studentRepo;
            _roomRepo = roomRepo;
        }

        public async Task<RoomAllocationResponse> AllocateRoomAsync(RoomAllocationRequest request)
        {
            var student = await _studentRepo.GetByIdAsync(request.StudentId);
            if (student == null)
                throw new Exception("Student not found");

            var room = await _roomRepo.GetByIdAsync(request.RoomId);
            if (room == null)
                throw new Exception("Room not found");

            if (room.CurrentOccupancy >= room.Capacity)
                throw new Exception("Room is full");

            var allocation = new RoomAllocation
            {
                StudentId = request.StudentId,
                RoomId = request.RoomId,
                AllocationDate = request.AllocationDate,
                CheckoutDate = request.CheckoutDate
            };

            var created = await _allocationRepo.CreateAsync(allocation);

            // Update occupancy
            room.CurrentOccupancy++;
            await _roomRepo.UpdateAsync(room.RoomId, new()
            {
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                Floor = room.Floor,
                Capacity = room.Capacity,
                CurrentOccupancy = room.CurrentOccupancy
            });

            return ToResponse(created);
        }

        public async Task<RoomAllocationResponse> FreeRoomAsync(int allocationId)
        {
            var allocation = await _allocationRepo.GetByIdAsync(allocationId);
            if (allocation == null)
                throw new Exception("Allocation not found");

            var room = await _roomRepo.GetByIdAsync(allocation.RoomId);
            if (room == null)
                throw new Exception("Room not found");

            if (room.CurrentOccupancy > 0)
                room.CurrentOccupancy--;

            allocation.CheckoutDate = DateTime.Now;
            await _allocationRepo.UpdateAsync(allocation);

            await _roomRepo.UpdateAsync(room.RoomId, new()
            {
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                Floor = room.Floor,
                Capacity = room.Capacity,
                CurrentOccupancy = room.CurrentOccupancy
            });

            return ToResponse(allocation);
        }

        public async Task<IEnumerable<RoomAllocationResponse>> GetByStudentAsync(int studentId)
        {
            var records = await _allocationRepo.GetAllocationsByStudent(studentId);
            return records.Select(x => ToResponse(x));
        }

        public async Task<IEnumerable<RoomAllocationResponse>> GetByRoomAsync(int roomId)
        {
            var records = await _allocationRepo.GetAllocationsByRoom(roomId);
            return records.Select(x => ToResponse(x));
        }

        public async Task<RoomAllocationResponse?> GetByIdAsync(int id)
        {
            var entity = await _allocationRepo.GetByIdAsync(id);
            return entity == null ? null : ToResponse(entity);
        }

        public async Task<IEnumerable<RoomAllocationResponse>> GetAllAsync()
        {
            var list = await _allocationRepo.GetAllAsync();
            return list.Select(x => ToResponse(x));
        }


        public async Task<bool> DeleteAsync(int id)    // ← ADD THIS
        {
            return await _allocationRepo.DeleteAsync(id);
        }
        // Mapping method (NO AutoMapper)
        private RoomAllocationResponse ToResponse(RoomAllocation entity)
        {
            return new RoomAllocationResponse
            {
                RoomAllocationId = entity.RoomAllocationId,
                StudentId = entity.StudentId,
                RoomId = entity.RoomId,
                AllocationDate = entity.AllocationDate,
                CheckoutDate = entity.CheckoutDate,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt,
                UpdatedBy = entity.UpdatedBy,

                Student = entity.Student == null ? null : new StudentResponse
                {
                    StudentId = entity.Student.StudentId,
                    Name = entity.Student.Name,
                    Gender = entity.Student.Gender,
                    Email = entity.Student.Email,
                    Phone = entity.Student.Phone,
                    Address = entity.Student.Address,
                    DateOfAdmission = entity.Student.DateOfAdmission
                },

                Room = entity.Room == null ? null : new RoomResponse
                {
                    RoomId = entity.Room.RoomId,
                    RoomNumber = entity.Room.RoomNumber,
                    RoomType = entity.Room.RoomType,
                    Floor = entity.Room.Floor,
                    Capacity = entity.Room.Capacity,
                    CurrentOccupancy = entity.Room.CurrentOccupancy
                }
            };
        }
    }
}
