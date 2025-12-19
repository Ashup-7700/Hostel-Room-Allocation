using System.ComponentModel.DataAnnotations;

namespace Kemar.HRM.Model.Request
{
    public class StudentRequest
    {
        public int? StudentId { get; set; }

        [ MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [ MaxLength(10)]
        public string Gender { get; set; } = string.Empty;

        [MaxLength(15)]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress, MaxLength(120)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;
        public DateTime DateOfAdmission { get; set; }

        public bool IsActive { get; set; } = true;



    }
}
