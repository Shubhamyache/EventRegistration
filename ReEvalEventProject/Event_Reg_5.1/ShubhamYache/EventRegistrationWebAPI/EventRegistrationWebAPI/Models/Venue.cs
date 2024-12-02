using System.ComponentModel.DataAnnotations;

namespace EventRegistrationWebAPI.Models
{
    public class Venue
    {
        [Key]
        public int VenueId { get; set; }

        [Required, MinLength(3)]
        public string VenueName { get; set; }
        [Required, MinLength(10)]
        public string AddressLine1 { get; set; }

        [Required, MinLength(3)]
        public string City { get; set; }
        
        [Required]
        public int MaxCapacity { get; set; }
        [Required]
        public bool HasSeats { get; set; }

        // Navigation property
        public ICollection<Event> Events { get; set; }
    }
}