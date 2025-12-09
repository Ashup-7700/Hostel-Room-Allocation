namespace Kemar.HRM.Model.Response
{
    public class RoomAllocationResponse
    {
        public int RoomAllocationId { get; set; }
        public int StudentId { get; set; }
        public int RoomId { get; set; }
        public int AllocatedByUserId { get; set; }
        public string? AllocatedBy { get; set; }
        public DateTime AllocatedAt { get; set; }
        public DateTime? ReleasedAt { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
