namespace Kemar.HRM.Model.Response
{
    public class FeeStructureResponse
    {
        public int FeeStructureId { get; set; }
        public string RoomType { get; set; } = string.Empty;
        public decimal MonthlyRent { get; set; }
        public decimal SecurityDeposit { get; set; }

        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

    }
}