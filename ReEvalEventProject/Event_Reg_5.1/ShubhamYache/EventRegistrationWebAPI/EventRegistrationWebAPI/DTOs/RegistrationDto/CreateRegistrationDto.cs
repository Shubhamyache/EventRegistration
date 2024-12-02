using System.ComponentModel.DataAnnotations;

namespace EventRegistrationWebAPI.DTOs.RegistrationDto
{
    public class CreateRegistrationDto
    {
       
        public string UserId { get; set; }
        public int EventId { get; set; }
        [Required, DataType(DataType.DateTime)]
        public DateTime RegistrationDateTime { get; set; }
        [Required]
        public string RegistrationStatus { get; set; }

        [Required, Range(0, 10)]
        public int PlatinumTicketsCount { get; set; }
        [Required, Range(0, 10)]
        public int GoldTicketsCount { get; set; }
        [Required, Range(0, 10)]
        public int SilverTicketsCount { get; set; }

        public DateTime? ApproveDate { get; set; }
        public CreatePaymentDto PaymentDto { get; set; }
    }
}
