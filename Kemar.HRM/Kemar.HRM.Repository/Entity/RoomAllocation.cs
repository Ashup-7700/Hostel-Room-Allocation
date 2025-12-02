using Kemar.HRM.Repository.Entity.BaseEntities;

namespace Kemar.HRM.Repository.Entity
{
    public class RoomAllocation : BaseEntity
    {
        public int RoomAllocationId { get; set; }
        public int StudentId { get; set; }
        public int RoomId { get; set; }

        public DateTime AllocationDate { get; set; }
        public DateTime? CheckoutDate { get; set; }

        public Student Student { get; set; }
        public Room Room { get; set; }
    }
}