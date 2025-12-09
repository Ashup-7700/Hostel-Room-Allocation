namespace Kemar.HRM.Model.Request
{
    public class UserRequest
    {
        public int? UserId { get; set; } 
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "Admin";
        public bool? IsActive { get; set; } = true;
    }
}
