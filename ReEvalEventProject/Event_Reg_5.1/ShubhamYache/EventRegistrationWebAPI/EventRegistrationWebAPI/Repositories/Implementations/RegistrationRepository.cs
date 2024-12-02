using EventRegistrationWebAPI.Data;
using EventRegistrationWebAPI.DTOs.RegistrationDto;
using EventRegistrationWebAPI.Models;
using EventRegistrationWebAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace EventRegistrationWebAPI.Repositories.Implementations
{
    public class RegistrationRepository : GenericRepository<Registration>, IRegistrationRepository
    {
        private readonly ApplicationDbContext _context;
        public RegistrationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Registration>> GetAllRegistrations()
        {
            
            return await _context.Set<Registration>()
                .Include(r => r.Event)
                .Include(r => r.User)
                .Include(r => r.Payment)
                .ToListAsync();

        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByUserIdAsync(string userId)
        {
            return await _dbSet.Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task<Registration?> AddRegistration(Registration registration, Payment payment)
        {
            if (registration == null)
            {
                return null;
            }

            // Add registration
            await _context.Registrations.AddAsync(registration);
            await _context.SaveChangesAsync();

            // Link payment to the registration
            payment.RegistrationId = registration.RegistrationId;
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            // Reload the registration with its payment to ensure it's included
            var registrationWithPayment = await _context.Registrations
                .Include(r => r.Payment)
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r => r.RegistrationId == registration.RegistrationId);

            return registrationWithPayment;
        }


        public async Task<Registration> CancelRegistrationAsync(int registrationId, Registration registration)
        {
            var oldPayments = _context.Payments.Where(payment => payment.RegistrationId == registrationId).ToList();
            var oldCount = oldPayments.Count;
            
            registration.RegistrationStatus = "Cancelled";
            var countReg = await _context.SaveChangesAsync();
            
            foreach (Payment payment in oldPayments)
            {
                payment.PaymentStatus = "Refund Inialized";
            }
            
            _context.Payments.UpdateRange(oldPayments);
            var countPay = await _context.SaveChangesAsync();

            if(countReg > 0 && oldCount == countPay)
            {
                return registration;
            }
            return null;
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByEventId(int eventId)
        {
            return await _context.Registrations
                                 .Where(r => r.EventId == eventId)
                                 .ToListAsync();
        }
    }
}
