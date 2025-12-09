namespace Kemar.HRM.Model.Filter
{
    public class RoomAllocationFilter
    {
        public int? StudentId { get; set; }
        public int? RoomId { get; set; }
        public bool? IsActive { get; set; }  // active allocations
        public DateTime? FromDate { get; set; } // allocation date range
        public DateTime? ToDate { get; set; }
    }
}
