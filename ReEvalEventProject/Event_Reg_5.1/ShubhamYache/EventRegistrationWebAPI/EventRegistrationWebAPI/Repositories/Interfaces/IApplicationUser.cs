using EventRegistrationWebAPI.Models;
using EventRegistrationWebAPI.Repositories.Implementations;

namespace EventRegistrationWebAPI.Repositories.Interfaces
{
    public interface IApplicationUser: IGenericRepository<ApplicationUser>
    {
         Task<ApplicationUser?> GetByEmailAsync(string email);
    }
}
