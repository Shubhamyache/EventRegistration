using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EventRegistrationWebAPI.DTOs
{
    public class PaymentDto
    {
        [Key]
        public int PaymentId { get; set; }
        public int RegistrationId { get; set; }
        [DataType(DataType.Date)]
        public DateTime PaymentDateTime { get; set; }
        [Required]
        public decimal PaymentAmount { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        [Required]
        public string PaymentStatus { get; set; }
        [Required]
        public string TypeOfTransaction { get; set; }
    }
}
