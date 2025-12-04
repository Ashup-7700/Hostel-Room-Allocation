namespace Kemar.HRM.Model.Request
{
    public class PaymentRequest
    {
        public int? PaymentId { get; set; }

        public int StudentId { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;
        public string PaymentType { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public bool? IsActive { get; set; }

    }
}
