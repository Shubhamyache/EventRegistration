using EventRegistrationWebAPI.Models;
using EventRegistrationWebAPI.Repositories.Implementations;

namespace EventRegistrationWebAPI.Repositories.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetPaymentsByRegistrationIdAsync(int registrationId);
    }

}
