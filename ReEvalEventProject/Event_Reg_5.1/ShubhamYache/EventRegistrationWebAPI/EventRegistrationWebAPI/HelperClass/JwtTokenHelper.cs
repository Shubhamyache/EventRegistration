using EventRegistrationWebAPI.Data;
using EventRegistrationWebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EventRegistrationWebAPI.HelperClass
{
    public class JwtTokenHelper
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public JwtTokenHelper(IConfiguration configuration, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
        }

        public async Task<string> GenerateToken(ApplicationUser appUser)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(appUser);
            var user = await _userManager.FindByEmailAsync(appUser.Email);

            //write an query which fetch the user from Users table the same email which user has
            var realUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == appUser.Email);    

            if (realUser == null)
            {
                throw new Exception("User not found");
            }

                      

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, appUser.Id),
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault()??""), //Use ClaimTypes.Role
                new Claim("Role", roles.FirstOrDefault()), //Use ClaimTypes.Role
                new Claim("FirstName", realUser.FirstName?? ""),
                new Claim("LastName", realUser.LastName?? ""),
                new Claim("UserId", realUser.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Aud, jwtSettings["Audience"]),
                new Claim(JwtRegisteredClaimNames.Iss, jwtSettings["Issuer"])
            };

            var tokenOptions = new JwtSecurityToken(
                
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationMinutes"])),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
