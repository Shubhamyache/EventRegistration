using EventRegistrationWebAPI.Data;
using EventRegistrationWebAPI.DTOs.AuthDto;
using EventRegistrationWebAPI.HelperClass;
using EventRegistrationWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EventRegistrationWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenHelper _jwtTokenHelper;
        
        

        public AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, JwtTokenHelper jwtTokenHelper, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtTokenHelper = jwtTokenHelper;
            
        }

        [HttpGet("checkUserNameExists")]
        public async Task<IActionResult> CheckUserEmailExists([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                return BadRequest(new Response { Status = "Exists", Message = "User already exists" });
            }

            return Ok(new Response { Status = "NotFound", Message = "User does not exist" });
        }

        [HttpPost("register/user")]
        public async Task<IActionResult> RegisterUser(RegistrationAuthDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if(!(await _roleManager.RoleExistsAsync("User")))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            await _userManager.AddToRoleAsync(user, "User");

            if (result.Succeeded)
            {
                return Ok(new Response{ Status="success", Message = "User registered successfully." });
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("register/organizer")]
        public async Task<IActionResult> RegisterOrganizer(RegistrationAuthDto organizerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var organizer = new ApplicationUser
            {
                FirstName = organizerDto.FirstName,
                LastName = organizerDto.LastName,
                UserName = organizerDto.Email,
                Email = organizerDto.Email,
                PhoneNumber = organizerDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(organizer, organizerDto.Password);

            if (!(await _roleManager.RoleExistsAsync("Organizer")))
            {
                await _roleManager.CreateAsync(new IdentityRole("Organizer"));
            }

            await _userManager.AddToRoleAsync(organizer, "Organizer");

            if (result.Succeeded)
            {
                return Ok(new Response{ Status="Success", Message = "Organizer registered successfully" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin(RegistrationAuthDto adminDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var admin = new ApplicationUser
            {
                FirstName = adminDto.FirstName,
                LastName = adminDto.LastName,
                UserName = adminDto.Email,
                Email = adminDto.Email,
                PhoneNumber = adminDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(admin, adminDto.Password);

            if (!(await _roleManager.RoleExistsAsync("Admin")))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            await _userManager.AddToRoleAsync(admin, "Admin");

            if (result.Succeeded)
            {
                return Ok(new Response{ Status="Success", Message = "Admin registered successfully" });
            }

            return BadRequest(result.Errors);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
  

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return Unauthorized("User does not exist !");
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, lockoutOnFailure:true);
           
            if (result.Succeeded)
            {
                var token = await _jwtTokenHelper.GenerateToken(user);
                return Ok(new Response{ Status="Success", Message="Login successful", Token = token  });
            }
            else if(result.IsLockedOut)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Account is Locked. Please try again later");
            }
            else {
                return Unauthorized("Invalid login Attempt");
            }           
        }
    }

}