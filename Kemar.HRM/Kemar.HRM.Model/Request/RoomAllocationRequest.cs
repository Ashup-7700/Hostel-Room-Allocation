using System.ComponentModel.DataAnnotations;

namespace Kemar.HRM.Model.Request
{
    public class RoomAllocationRequest
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public DateTime AllocationDate { get; set; }

        public DateTime? CheckoutDate { get; set; }
    }
}