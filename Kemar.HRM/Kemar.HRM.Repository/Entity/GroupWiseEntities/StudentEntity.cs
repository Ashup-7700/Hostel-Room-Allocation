using Kemar.HRM.Repository.Entity.BaseEntities;
using System;

using System.ComponentModel.DataAnnotations;


namespace Kemar.HRM.Repository.Entity.GroupWiseEntities
{
    public class StudentEntity :BaseEntity
    {

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        public string Gender { get; set; }

        [Required]
        [MaxLength(15)]
        public string Phone { get; set; }


        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }


        [MaxLength(200)]
        public string Address { get; set; }

        [Required]
        public DateTime DateOfAdmission { get; set; }




        // Many Students --> One Room 
        public int? RoomId { get; set; }
        public RoomEntity Room { get; set; }




    }
}
