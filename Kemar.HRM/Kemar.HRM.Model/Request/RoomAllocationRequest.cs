using System.ComponentModel.DataAnnotations;

namespace Kemar.HRM.Model.Request
{
    public class RoomAllocationRequest
    {
        public int RoomAllocationId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int AllocatedByUserId { get; set; }

        [Required]
        public DateTime AllocatedAt { get; set; }

        public DateTime? ReleasedAt { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
