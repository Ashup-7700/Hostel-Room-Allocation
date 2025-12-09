using System.Text.Json.Serialization;

namespace Kemar.HRM.Model.Request
{
    public class RoomAllocationRequest
    {
        public int? RoomAllocationId { get; set; }

        public int StudentId { get; set; }
        public int RoomId { get; set; }

        [JsonIgnore]  // Set automatically from logged-in user
        public int? AllocatedByUserId { get; set; }

        public DateTime? AllocatedAt { get; set; }
        public DateTime? ReleasedAt { get; set; }

        public bool IsActive { get; set; }
    }
}
