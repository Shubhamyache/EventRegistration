using EventRegistrationWebAPI.DTOs;
using EventRegistrationWebAPI.DTOs.RegistrationDto;
using EventRegistrationWebAPI.Models;
using EventRegistrationWebAPI.Repositories.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace EventRegistrationWebAPI.Repositories.Interfaces
{
    public interface IRegistrationRepository : IGenericRepository<Registration>
    {
        Task<IEnumerable<Registration>> GetAllRegistrations();
        Task<IEnumerable<Registration>> GetRegistrationsByUserIdAsync(string userId);
        Task<Registration> AddRegistration(Registration registration, Payment payments);
     
        Task<Registration> CancelRegistrationAsync(int registrationId, Registration registration);

       Task<IEnumerable<Registration>> GetRegistrationsByEventId(int eventId);


    }
}
