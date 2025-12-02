using Kemar.HRM.Repository.Entity.BaseEntities;

namespace Kemar.HRM.Repository.Entity
{
    public class Room : BaseEntity
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public int Floor { get; set; }
        public int Capacity { get; set; }
        public int CurrentOccupancy { get; set; }

        public ICollection<RoomAllocation>? RoomAllocations { get; set; } 
    }
}