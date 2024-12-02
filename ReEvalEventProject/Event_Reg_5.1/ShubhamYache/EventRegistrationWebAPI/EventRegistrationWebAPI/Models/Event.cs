using EventRegistrationWebAPI.CustomValidations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EventRegistrationWebAPI.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required, MinLength(4)]
        public string EventName { get; set; }

        [Required, MinLength(4)]
        public string Category { get; set; }


        [Required, DataType(DataType.Date)]
        [FutureDate]
        public DateTime EventStartDateTime { get; set; }

        [Required, DataType(DataType.Date)]
        [FutureDate]
        public DateTime EventEndDateTime { get; set; }
        [Required, DataType(DataType.Date)]
        [FutureDate]
        public DateTime RegistrationCloseDate { get; set; }

        [Required, Range(0,21)]
        public int MinimumAge {  get; set; }

        [Required]
        public int PlatinumTicketsNumber {  get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PlatinumTicketsPrice { get; set; }
        [Required]
        public int GoldTicketsNumber { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal GoldTicketsPrice { get;set; }
        [Required]
        public int SilverTicketsNumber { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal SilverTicketsPrice { get; set; }

        [Required]
        public string EventStatus { get; set; }
        [Required]
        public string Hashtag { get; set; }
        [Required]
        public string Description {  get; set; }

        [Required]
        public string? ImageUrl { get; set; }

        [ForeignKey("Venue")]
        public int VenueId { get; set; }
        public Venue Venue { get; set; }

        public string OrganizerId { get; set; }
        
        public ApplicationUser Organizer { get; set; }

        public ICollection<Registration> Registrations { get; set; }

        //oversubscription fields
        public int PlatinumTicketsOversubscribed { get; set; } = 0;
        public int GoldTicketsOversubscribed { get; set; } = 0;
        public int SilverTicketsOversubscribed { get; set; } = 0;

    }

    //public enum EventStatus
    //{
    //    Draft,
    //    Published,
    //    Cancelled,
    //    Closed
    //}
}