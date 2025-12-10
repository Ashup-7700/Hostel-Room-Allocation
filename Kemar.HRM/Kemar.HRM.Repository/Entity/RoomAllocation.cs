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

        public DateTime AllocatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReleasedAt { get; set; }

        public bool IsActive { get; set; } = true;

        public int AllocatedByUserId { get; set; }
        public User? AllocatedByUser { get; set; }
    }
}
