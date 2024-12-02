using EventRegistrationWebAPI.Data;
using EventRegistrationWebAPI.Models;
using EventRegistrationWebAPI.Repositories.Interfaces;

namespace EventRegistrationWebAPI.Repositories.Implementations
{
    public class VenueRepository :GenericRepository<Venue>, IVenueRepository
    {
        public VenueRepository(ApplicationDbContext context) : base(context)
        {
        }
    }

}
