using EventRegistrationWebAPI.Data;
using EventRegistrationWebAPI.Repositories.Interfaces;

namespace EventRegistrationWebAPI.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IEventRepository Events { get; private set; }
        public IRegistrationRepository Registrations { get; private set; }
        public IPaymentRepository Payments { get; private set; }
        public IVenueRepository Venues { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Events = new EventRepository(_context);
            Registrations = new RegistrationRepository(_context);
            Payments = new PaymentRepository(_context);
            Venues = new VenueRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
