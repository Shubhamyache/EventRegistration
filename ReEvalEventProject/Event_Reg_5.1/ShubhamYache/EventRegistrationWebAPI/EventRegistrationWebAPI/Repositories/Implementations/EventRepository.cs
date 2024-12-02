using EventRegistrationWebAPI.Data;
using EventRegistrationWebAPI.DTOs.EventDto;
using EventRegistrationWebAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventRegistrationWebAPI.Repositories.Implementations
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Event>> GetActiveEventsAsync()
        {
            return await _dbSet
                .Where(e => e.EventStartDateTime >= DateTime.Now && e.EventStatus == "Published")
                .Include(e => e.Organizer)
                .Include(e => e.Venue)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetActiveEventsByCityAsync(string city)
        {
            if(!string.IsNullOrWhiteSpace(city))
            {
                return await _dbSet.Where(e => e.EventStartDateTime >= DateTime.Now && e.Venue.City.ToLower()==city).ToListAsync();
            }
            return new List<Event>();
        }
        public async Task<IEnumerable<Event>> GetAllEvents()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Event?> GetEventById(int eventId)
        {
            return await _dbSet.Where(e => e.EventId == eventId)
                .Include(e=>e.Organizer)
                .Include(e=>e.Venue)
                .FirstOrDefaultAsync();
          
        }

    }
}
