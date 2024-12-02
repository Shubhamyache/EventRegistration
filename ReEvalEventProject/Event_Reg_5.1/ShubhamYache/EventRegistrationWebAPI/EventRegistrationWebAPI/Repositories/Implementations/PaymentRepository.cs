using EventRegistrationWebAPI.Data;
using EventRegistrationWebAPI.Models;
using EventRegistrationWebAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventRegistrationWebAPI.Repositories.Implementations
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Payment>> GetPaymentsByRegistrationIdAsync(int registrationId)
        {
            return await _dbSet.Where(p => p.RegistrationId == registrationId).ToListAsync();
        }
    }

}
