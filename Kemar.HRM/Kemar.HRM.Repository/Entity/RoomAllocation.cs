using Kemar.HRM.Repository.Entity.BaseEntities;

namespace Kemar.HRM.Repository.Entity
{
    public class RoomAllocation : BaseEntity
    {
        public int RoomAllocationId { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int RoomId { get; set; }
        public Room? Room { get; set; }

        // link to user who allocated
        public int AllocatedByUserId { get; set; }
        public User? AllocatedBy { get; set; }

        public DateTime AllocatedAt { get; set; }
        public DateTime? ReleasedAt { get; set; }
    }
}
