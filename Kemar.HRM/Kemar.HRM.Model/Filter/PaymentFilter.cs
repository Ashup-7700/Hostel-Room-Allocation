namespace Kemar.HRM.Model.Filter
{
    public class PaymentFilter
    {
        public int? StudentId { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentType { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
}
