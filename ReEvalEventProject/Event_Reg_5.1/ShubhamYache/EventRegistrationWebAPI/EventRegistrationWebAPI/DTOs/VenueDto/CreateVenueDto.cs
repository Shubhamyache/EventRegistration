using System.ComponentModel.DataAnnotations;

namespace EventRegistrationWebAPI.DTOs.VenueDto
{
    public class CreateVenueDto
    {
        public string VenueName { get; set; }
        [Required, MinLength(3)]
        public string City { get; set; }
        [Required, MinLength(3)]
        public string AddressLine1 { get; set; }
        [Required]
        public int MaxCapacity { get; set; }
        [Required]
        public bool HasSeats { get; set; }
    }
}
