using System.ComponentModel.DataAnnotations;

namespace Kemar.HRM.Model.Request
{
    public class RoomRequest
    {
        [Required]
        [MaxLength(20)]
        public string? RoomNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string? RoomType { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Floor must be valid.")]
        public int Floor { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Capacity must be at least 1.")]
        public int Capacity { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Current occupancy must be valid.")]
        public int CurrentOccupancy { get; set; }
    }
}