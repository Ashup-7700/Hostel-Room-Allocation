using Kemar.HRM.Repository.Entity.BaseEntities;

namespace Kemar.HRM.Repository.Entity
{
    public class Payment : BaseEntity
    {
        public int PaymentId { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMode { get; set; } = string.Empty;

        public string PaymentStatus { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public int? CreatedByUserId { get; set; }

    }
}
