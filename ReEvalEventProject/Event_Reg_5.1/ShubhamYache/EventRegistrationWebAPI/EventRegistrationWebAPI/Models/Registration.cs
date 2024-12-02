using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventRegistrationWebAPI.Models
{
    public class Registration
    {
        [Key]
        public int RegistrationId { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }
        public Event Event { get; set; }

        [Required, DataType(DataType.DateTime)]
        [CustomValidation(typeof(Registration), "ValidateRegistrationDateTime")]
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

        public int? PaymentId { get; set; }
        public Payment Payment { get; set; }

        public static ValidationResult ValidateRegistrationDateTime(DateTime registrationDateTime, ValidationContext context)
        {
            if (registrationDateTime > DateTime.Now)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Registration date should not be a past date.");
            }
        }
    }
    //not used
    public enum RegistrationStatus
    {
        Pending,
        Completed,
        Cancelled
    }
}