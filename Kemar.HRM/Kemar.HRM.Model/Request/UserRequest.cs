namespace Kemar.HRM.Model.Request
{
    public class UserRequest
    {
        public int? UserId { get; set; } // null for create, value for update
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "Admin";
        public bool? IsActive { get; set; } = true;
    }
}
