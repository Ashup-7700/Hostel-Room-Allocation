using System.ComponentModel.DataAnnotations;

namespace Kemar.HRM.Model.Request
{
    public class StudentRequest
    {
        [Required]
        [MaxLength(25)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(8)]
        public string? Gender { get; set; }

        [Required]
        [MaxLength(15)]
        public string? Phone { get; set; }

        [Required]
        [MaxLength(25)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Address { get; set; }

        [Required]
        public DateTime DateOfAdmission { get; set; }
    }
}