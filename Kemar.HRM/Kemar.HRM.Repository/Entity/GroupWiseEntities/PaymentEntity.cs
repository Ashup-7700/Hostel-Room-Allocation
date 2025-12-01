using Kemar.HRM.Repository.Entity.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.HRM.Repository.Entity.GroupWiseEntities
{
     public class PaymentEntity : BaseEntity
    {
        [Required]
        public int StudentId { get; set; }  // Which student made the payment

        public StudentEntity Student { get; set; }  // Navigation property 


        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be grater than 0")]
        public decimal Amount { get; set; }


        [Required]
        public DateTime PaymentDate { get; set; }  //When the payment was made


        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; }  //Cash, Card, UPI


        [Required]
        [MaxLength(50)]
        public string PaymentType { get; set; }  // Admission fee, Monthly Fee, Hostel Fee



    }
}
