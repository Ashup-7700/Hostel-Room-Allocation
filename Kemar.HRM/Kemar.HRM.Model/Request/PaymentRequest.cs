namespace Kemar.HRM.Model.Request
{
    public class PaymentRequest
    {
        public int PaymentId { get; set; }

        public int StudentId { get; set; }

        // 🔹 Total amount (calculated from RoomAllocation)
        public decimal TotalAmount { get; set; }

        // 🔹 Amount user is paying now / total paid
        public decimal PaidAmount { get; set; }

        public string PaymentMode { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        // 🔹 Status will be set in backend (Pending / Completed)
        public string? PaymentStatus { get; set; }

        public int? CreatedByUserId { get; set; }
        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        // 🔹 Remaining amount property
        public decimal RemainingAmount => TotalAmount - PaidAmount;
    }
}
