using System;
using System.ComponentModel.DataAnnotations;

namespace Kemar.HRM.Repository.Entity.BaseEntities
{
    public class BaseEntity
    {

        public int Id {  get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; } = "Active";


    }
}
