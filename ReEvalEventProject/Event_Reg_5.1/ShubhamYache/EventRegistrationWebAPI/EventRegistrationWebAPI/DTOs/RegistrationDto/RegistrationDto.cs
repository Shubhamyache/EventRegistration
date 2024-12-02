using System.ComponentModel.DataAnnotations;
using EventRegistrationWebAPI.Models;

namespace EventRegistrationWebAPI.DTOs.RegistrationDto
{
    public class RegistrationDto
    {
        public int RegistrationId { get; set; }
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
        public PaymentDto PaymentDto { get; set; }
    }
}
