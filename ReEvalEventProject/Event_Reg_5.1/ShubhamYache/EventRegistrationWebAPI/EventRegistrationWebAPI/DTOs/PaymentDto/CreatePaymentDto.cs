using System.ComponentModel.DataAnnotations;

namespace EventRegistrationWebAPI.DTOs
{
    public class CreatePaymentDto
    {
        [Required]
        public decimal PaymentAmount { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        [Required]
        public DateTime PaymentDateTime { get; set; }
        [Required]
        public string PaymentStatus { get; set; }
        [Required]
        public string TypeOfTransaction { get; set; }
       
    }
}
