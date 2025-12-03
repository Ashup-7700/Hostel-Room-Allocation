using Kemar.HRM.Repository.Entity.BaseEntities;

namespace Kemar.HRM.Repository.Entity
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;

        public string Role { get; set; } = "Warden";

        public ICollection<RoomAllocation>? AllocationsHandled { get; set; }
    }
}
