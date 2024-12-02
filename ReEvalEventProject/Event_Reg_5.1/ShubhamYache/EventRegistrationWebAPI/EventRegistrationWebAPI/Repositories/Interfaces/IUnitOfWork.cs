using EventRegistrationWebAPI.Repositories.Implementations;

namespace EventRegistrationWebAPI.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEventRepository Events { get; }
        IRegistrationRepository Registrations { get; }
        IPaymentRepository Payments { get; }
        IVenueRepository Venues { get; }
        Task<int> CompleteAsync();
    }

}
