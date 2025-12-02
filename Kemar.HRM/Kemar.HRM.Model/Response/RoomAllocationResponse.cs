namespace Kemar.HRM.Model.Response
{
    public class RoomAllocationResponse
    {
        public int RoomAllocationId { get; set; }

        public int StudentId { get; set; }
        public int RoomId { get; set; }

        public DateTime AllocationDate { get; set; }
        public DateTime? CheckoutDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public StudentResponse? Student { get; set; }
        public RoomResponse? Room { get; set; }
    }
}