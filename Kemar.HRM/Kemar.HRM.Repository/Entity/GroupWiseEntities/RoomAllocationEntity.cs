using Kemar.HRM.Repository.Entity.BaseEntities;
using System;

using System.ComponentModel.DataAnnotations;


namespace Kemar.HRM.Repository.Entity.GroupWiseEntities
{
     public class RoomAllocationEntity : BaseEntity
    {

        [Required]
        public int StudentId { get; set; }  // Foregin Key


        [Required]
        public int RoomId { get; set; }   // Foregin Key


        public StudentEntity Student { get; set; }  // Navigation property for EF Core

        public RoomEntity Room { get; set; }  // Navigation property to the room


        [Required]
        public DateTime AllocationDate { get; set; }  // date was given the room

        public DateTime? CheckoutDate { get; set; }  // student may still be living in the room

    }
}
