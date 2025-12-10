namespace Kemar.HRM.Model.Filter
{
    public class RoomAllocationFilter
    {
        public int? StudentId { get; set; }    // Filter by specific student
        public int? RoomId { get; set; }       // Filter by specific room
        public bool? IsActive { get; set; }    // Filter by active/inactive allocations
    }
}
