using System.ComponentModel.DataAnnotations;

namespace EventRegistrationWebAPI.DTOs.VenueDto
{
    public class VenueDto
    {
        [Key]
        public int VenueId { get; set; }
        [Required, MinLength(3)]
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
