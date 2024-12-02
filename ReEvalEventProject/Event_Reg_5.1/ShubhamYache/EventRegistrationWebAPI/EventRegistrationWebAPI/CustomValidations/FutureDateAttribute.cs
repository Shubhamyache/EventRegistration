using System.ComponentModel.DataAnnotations;

namespace EventRegistrationWebAPI.CustomValidations
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateValue = (DateTime)value;

            if (dateValue < DateTime.Now)
            {
                return new ValidationResult("The date cannot be in the past.");
            }

            return ValidationResult.Success;
        }
    }

}
