using EventRegistrationWebAPI.Models;

namespace EventRegistrationWebAPI.DTOs.UserDto
{
    public class UserEventDto
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<Event> Events { get; set; }

    }
}
