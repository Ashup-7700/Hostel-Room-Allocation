namespace Kemar.HRM.Model.Response
{
    public class PaymentResponse
    {
        public int PaymentId { get; set; }

        public int StudentId { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }

        public decimal RemainingAmount => TotalAmount - PaidAmount;

        public string PaymentMode { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }

        public string PaymentStatus { get; set; } = string.Empty;

        public int? CreatedByUserId { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }
    }
}
