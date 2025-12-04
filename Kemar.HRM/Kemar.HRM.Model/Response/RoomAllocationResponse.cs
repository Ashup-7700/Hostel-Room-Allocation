using System;

namespace Kemar.HRM.Model.Response
{
    public class RoomAllocationResponse
    {
        public int RoomAllocationId { get; set; }

        public int StudentId { get; set; }
        public StudentResponse? Student { get; set; }

        public int RoomId { get; set; }
        public RoomResponse? Room { get; set; }

        public int AllocatedByUserId { get; set; }
        public UserResponse? AllocatedBy { get; set; }

        public DateTime AllocatedAt { get; set; }
        public DateTime? ReleasedAt { get; set; }

        // Audit
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

    }
}
