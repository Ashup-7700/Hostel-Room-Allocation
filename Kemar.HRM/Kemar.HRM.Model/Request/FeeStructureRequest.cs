using System.ComponentModel.DataAnnotations;

namespace Kemar.HRM.Model.Request
{
    public class FeeStructureRequest
    {
        public int FeeStructureId { get; set; }

        [Required(ErrorMessage = "Room Type is required.")]
        [MaxLength(50, ErrorMessage = "Room Type cannot exceed 50 characters.")]
        public string? RoomType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Monthly Rent is required.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Monthly Rent must be greater than or equal to 0.")]
        public decimal MonthlyRent { get; set; }

        [Required(ErrorMessage = "Security Deposit is required.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Security Deposit must be greater than or equal to 0.")]
        public decimal SecurityDeposit { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

    }
}