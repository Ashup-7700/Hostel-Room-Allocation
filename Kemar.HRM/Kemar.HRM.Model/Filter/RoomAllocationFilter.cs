namespace Kemar.HRM.Model.Filter
{
    public class RoomAllocationFilter
    {
        public int? StudentId { get; set; }
        public int? RoomId { get; set; }
        public int? AllocatedByUserId { get; set; }
        public bool? IsActive { get; set; }

        public DateTime? FromAllocatedAt { get; set; }
        public DateTime? ToAllocatedAt { get; set; }

    }
}
