using AutoMapper;
using EventRegistrationWebAPI.Data;
using EventRegistrationWebAPI.DTOs.UserDto;
using EventRegistrationWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventRegistrationWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUser()
        {
            try
            {
                var users = await _userManager.GetUsersInRoleAsync("User");
                return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving users.");
            }
        }

        [HttpGet("GetAllUsersWithEvents")]
        public async Task<ActionResult> GetAllUsersWithEvents()
        {
            try
            {
                var users = await _userManager.GetUsersInRoleAsync("User");
                var result = new List<object>();

                foreach (var user in users)
                {
                    var userRegistrations = await _context.Registrations
                        .Where(r => r.UserId == user.Id && r.Event.EventStatus == "Active")
                        .Select(r => r.Event)
                        .Distinct()
                        .ToListAsync();

                    // Simplified user with events, added email field
                    var userWithEvents = new
                    {
                        user = new
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email  // Adding email to the response
                        },
                        events = userRegistrations.Select(e => new
                        {
                            eventId = e.EventId,  // Only eventId, no $id
                            eventName = e.EventName,
                            eventStartDate = e.EventStartDateTime,
                            eventEndDate = e.EventEndDateTime
                        }).ToList()
                    };

                    result.Add(userWithEvents);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving users with events.");
            }
        }

    }
}
