using System.ComponentModel.DataAnnotations;

namespace Kemar.HRM.Model.Request
{
    public class UserRequest
    {
        public int? UserId { get; set; }

        [Required, MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(120)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string MobileNo { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Role { get; set; } = "Warden";

        public bool? IsActive { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
