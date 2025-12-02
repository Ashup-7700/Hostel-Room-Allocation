using System.ComponentModel.DataAnnotations;

namespace Kemar.HRM.Model.Request
{
    public class PaymentRequest
    {
        [Required(ErrorMessage = "StudentId is required.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "PaymentMethod is required.")]
        [MaxLength(50, ErrorMessage = "PaymentMethod cannot exceed 50 characters.")]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required(ErrorMessage = "PaymentType is required.")]
        [MaxLength(50, ErrorMessage = "PaymentType cannot exceed 50 characters.")]
        public string PaymentType { get; set; } = string.Empty;

        [Required(ErrorMessage = "PaymentDate is required.")]
        public DateTime PaymentDate { get; set; }
    }
}