using System;

namespace Kemar.HRM.Model.Request
{
    public class RoomAllocationRequest
    {
        public int RoomAllocationId { get; set; }
        public int StudentId { get; set; }
        public int RoomId { get; set; }
        public DateTime? ReleasedAt { get; set; }

        public int AllocatedByUserId { get; set; }
    }
}
