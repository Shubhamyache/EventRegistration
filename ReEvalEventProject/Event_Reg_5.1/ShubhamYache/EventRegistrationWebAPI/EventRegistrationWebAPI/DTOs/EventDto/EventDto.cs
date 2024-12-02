﻿using EventRegistrationWebAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EventRegistrationWebAPI.CustomValidations;

namespace EventRegistrationWebAPI.DTOs.EventDto
{
    public class EventDto
    {
        [Key]
        public int EventId { get; set; }

        [Required, MinLength(4)]
        public string EventName { get; set; }

        [Required, MinLength(4)]
        public string Category { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime EventStartDateTime { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime EventEndDateTime { get; set; }

        [Required, DataType(DataType.Date)]
        [FutureDate]
        public DateTime RegistrationCloseDate { get; set; }

        [Required, Range(0, 21)]
        public int MinimumAge { get; set; }

        [Required]
        public int PlatinumTicketsNumber { get; set; }
        [Required]
        public decimal PlatinumTicketsPrice { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public int GoldTicketsNumber { get; set; }
        [Required]
        public decimal GoldTicketsPrice { get; set; }
        [Required]
        public int SilverTicketsNumber { get; set; }
        [Required]
        public decimal SilverTicketsPrice { get; set; }
        [Required]
        public string EventStatus { get; set; }
        
        public string Hashtag { get; set; }
        [Required]
        public string? ImageUrl { get; set; }

        [Required]
        public string OrganizerEmail { get; set; }
        [Required]
        public int VenueId { get; set; }


        //Avoid circular reference 
        //public Venue Venue { get; set; }
        //public ApplicationUser ApplicationUser { get; set; }

    }
}