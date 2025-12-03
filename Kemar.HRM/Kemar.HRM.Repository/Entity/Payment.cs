using Kemar.HRM.Repository.Entity.BaseEntities;

namespace Kemar.HRM.Repository.Entity
{
    public class Payment : BaseEntity
    {
        public int PaymentId { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = string.Empty; // UPI, Cash, Card
        public string PaymentType { get; set; } = string.Empty;   // Monthly Rent / Security Deposit

        public DateTime PaymentDate { get; set; }
    }
}
