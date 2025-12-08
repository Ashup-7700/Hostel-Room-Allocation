namespace Kemar.HRM.Repository.Entity
{
     public class UserToken
    {
        public int UserTokenId { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime GeneratedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string? SystemIp { get; set; }
           
        public int UserId { get; set; }

    }
}
