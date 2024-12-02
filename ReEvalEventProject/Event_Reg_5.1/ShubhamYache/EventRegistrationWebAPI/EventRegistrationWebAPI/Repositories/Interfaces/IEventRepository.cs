using EventRegistrationWebAPI.DTOs.EventDto;
using EventRegistrationWebAPI.Models;

namespace EventRegistrationWebAPI.Repositories.Implementations
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Task<IEnumerable<Event>> GetActiveEventsAsync();
        Task<IEnumerable<Event>> GetActiveEventsByCityAsync(string city);
        Task<IEnumerable<Event>> GetAllEvents();
        Task<Event?> GetEventById(int eventId);
        // Task<Event> GetEventDetails();
    }

}
