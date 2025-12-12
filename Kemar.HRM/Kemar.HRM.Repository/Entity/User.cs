using Kemar.HRM.Repository.Entity.BaseEntities;

namespace Kemar.HRM.Repository.Entity
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = "Admin";  
        public string Password { get; set; } = string.Empty; 
        public bool IsActive { get; set; } = true;

        public  ICollection<RoomAllocation>? AllocationsHandled { get; set; }
    }
}
