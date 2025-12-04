using Kemar.HRM.Repository.Entity.BaseEntities;

namespace Kemar.HRM.Repository.Entity
{
    public class Student : BaseEntity
    {
        public int StudentId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime DateOfAdmission { get; set; }

        public ICollection<RoomAllocation>? RoomAllocations { get; set; }
        public ICollection<Payment>? Payments { get; set; }
    }
}
