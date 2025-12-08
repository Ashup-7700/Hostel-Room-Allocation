namespace Kemar.HRM.Model.Response
{
    public class UserLoginResponse
    {
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string? SystemIp { get; set; }

    }
}
