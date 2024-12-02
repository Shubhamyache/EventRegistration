using EventRegistrationWebAPI.Data;
using EventRegistrationWebAPI.Models;
using EventRegistrationWebAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EventRegistrationWebAPI.Repositories.Implementations
{
    public class ApplicationUserRepository : IApplicationUser
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public Task AddAsync(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            var appUser = await _userManager.FindByEmailAsync(email);
            if (appUser == null) {  
                return null;
            }
            return appUser;
           
        }

        public Task<ApplicationUser> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
