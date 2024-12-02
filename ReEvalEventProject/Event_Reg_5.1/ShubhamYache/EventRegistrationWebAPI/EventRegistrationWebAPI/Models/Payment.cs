
using EventRegistrationWebAPI.CustomValidations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventRegistrationWebAPI.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }


        [ForeignKey("Registration")]
        public int RegistrationId { get; set; }
        public Registration Registration { get; set; }


        [DataType(DataType.Date)]
        [FutureDate]
        public DateTime PaymentDateTime { get; set; } //payment date and time for the registration

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PaymentAmount { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        [Required]
        public string PaymentStatus { get; set; }
        [Required]
        public string TypeOfTransaction {  get; set; }
    }

    public enum PaymentStatus
    {
        Pending,
        Successful,
        Failed,
        FullyRefunded,
        PartiallyRefunded
    }
}