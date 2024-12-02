//using EventRegistrationSystem.Models;

using EventRegistrationWebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EventRegistrationWebAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, StringLength(70), MinLength(3)]
        public string FirstName { get; set; }
        [Required, StringLength(70), MinLength(3)]
        public string LastName { get; set; }
        public ICollection<Registration> Registrations { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}