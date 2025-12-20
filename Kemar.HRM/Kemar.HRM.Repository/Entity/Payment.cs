using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.Entity.BaseEntities;
using System.ComponentModel.DataAnnotations.Schema;

public class Payment : BaseEntity
{
    public int PaymentId { get; set; }

    public int StudentId { get; set; }
    public Student? Student { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }

    [NotMapped]
    public decimal RemainingAmount => TotalAmount - PaidAmount;

    [NotMapped]
    public string PaymentStatus => PaidAmount >= TotalAmount ? "Completed" : "Pending";

    public string PaymentMode { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public int? CreatedByUserId { get; set; }
}
