namespace Kemar.HRM.Model.Response
{
    public class RoomAllocationResponse
    {
        public int RoomAllocationId { get; set; }
        public int StudentId { get; set; }
        public int RoomId { get; set; }

        public DateTime? AllocatedAt { get; set; }
        public DateTime? ReleasedAt { get; set; }
        public bool IsActive { get; set; }

        public string StudentName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;

        public string AllocatedByUsername { get; set; } = string.Empty;
    }
}
