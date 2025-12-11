using System;

namespace Kemar.HRM.Model.Request
{
    public class RoomAllocationRequest
    {
        public int RoomAllocationId { get; set; }
        public int StudentId { get; set; }
        public int RoomId { get; set; }

        public int AllocatedByUserId { get; set; }  
        public DateTime AllocatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReleasedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
