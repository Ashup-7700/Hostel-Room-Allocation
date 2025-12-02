namespace Kemar.HRM.Model.Response
{
    public class StudentResponse
    {
        public int StudentId { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfAdmission { get; set; }

        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public int TotalRoomAllocations { get; set; }
        public int TotalPayments { get; set; }

    }
}