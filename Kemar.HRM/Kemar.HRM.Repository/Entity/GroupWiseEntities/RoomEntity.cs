using Kemar.HRM.Repository.Entity.BaseEntities;
using System;
using System.ComponentModel.DataAnnotations;


namespace Kemar.HRM.Repository.Entity.GroupWiseEntities
{
    public class RoomEntity : BaseEntity
    {
        [Required]
        [MaxLength(250)]
        public string RoomNumber { get; set; }   // A-101


        [Required]
        [MaxLength(70)]
        public string RoomType { get; set; }     //Single,Double

        [Required]
        public int Floor {  get; set; }     //1,2,3

        [Required]
        public int Capacity { get; set; }   //how many student is allow people

        [Required]
        public int CurrentOccupancy { get; set; }   //currently occupied the room                 0 <= CurrentOccupancy <= Capacity


        public ICollection<StudentEntity> Students { get; set; } = new List<StudentEntity>();


    }
}
