namespace Kemar.HRM.Model.Request
{
    public class PaymentRequest
    {
        public int PaymentId { get; set; }  

        public int StudentId { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMode { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public string PaymentStatus { get; set; } = string.Empty;

        public int? CreatedByUserId { get; set; } 
    }
}
