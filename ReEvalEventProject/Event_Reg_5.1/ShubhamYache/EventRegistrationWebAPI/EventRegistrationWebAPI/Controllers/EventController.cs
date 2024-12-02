using AutoMapper;
using EventRegistrationWebAPI.Data;
using EventRegistrationWebAPI.DTOs.EventDto;
using EventRegistrationWebAPI.Models;
using EventRegistrationWebAPI.Repositories.Implementations;
using EventRegistrationWebAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventRegistrationWebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EventController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IApplicationUser _applicationUser;
        private readonly IEventRepository _eventRepository;

        public EventController(IUnitOfWork unitOfWork, IMapper mapper,IEventRepository eventRepository ,ApplicationDbContext context, RoleManager<IdentityRole> roleManager, IApplicationUser applicationUser, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
           _roleManager = roleManager;
            _context = context;
            _applicationUser = applicationUser;
            _eventRepository = eventRepository;

        }

        [HttpGet]
        [Route("GetActiveEvents")]
        public async Task<IEnumerable<PostEventDto>> GetActiveEvents()
        {
            var events = await _unitOfWork.Events.GetActiveEventsAsync();
            return _mapper.Map<IEnumerable<PostEventDto>>(events);
        }


        [HttpGet]
        [Route("GetActiveEventsByCity")]
        public async Task<IEnumerable<PostEventDto>> GetActiveEventsByCity(string city)
        {
            city = city.ToLower();
            var events = await _unitOfWork.Events.GetActiveEventsByCityAsync(city);
            return _mapper.Map<IEnumerable<PostEventDto>>(events);
        }

        [HttpGet]
        [Route("GetEventById")]
        public async Task<PostEventDto> GetEventById(int eventId)
        {




            return _mapper.Map<PostEventDto>(await _unitOfWork.Events.GetEventById(eventId));
        }

        //[Authorize(Roles ="Organizer")]
        [HttpGet]
        [Route("GetAllEvents")]
        public async Task<IEnumerable<EventDto>> GetAllEvents()
        {
            var events = await _unitOfWork.Events.GetAllAsync();
            return _mapper.Map<IEnumerable<EventDto>>(events);
        }
       // [Authorize(Roles = "Organizer")]

        [HttpPost("AddEvent")]
        public async Task<IActionResult> AddEvent(CreateEventDto eventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Additional validation logic
            if (eventDto.EventStartDateTime < DateTime.Now)
            {
                return BadRequest("Event start date cannot be in the past.");
            }

            if (eventDto.EventEndDateTime <= eventDto.EventStartDateTime)
            {
                return BadRequest("Event end date must be after the start date.");
            }

            if (eventDto.RegistrationCloseDate >= eventDto.EventStartDateTime)
            {
                return BadRequest("Registration close date must be before the event start date.");
            }

            if (string.IsNullOrEmpty(eventDto.OrganizerEmail))
            {
                return BadRequest("Organizer email is required.");
            }
        

            // Find the organizer by email
           var organizer = await _applicationUser.GetByEmailAsync(eventDto.OrganizerEmail.ToLower());
            //var organizer = await _userManager.FindByEmailAsync(organizerEmail);

            if (organizer == null)
            {
                return BadRequest("Organizer not found. Please check the email and try again.");
            }
            var roles = await _userManager.GetRolesAsync(organizer);
            
            if (!roles.Contains("Organizer"))
            {
                return BadRequest("The user is not an organizer.");
            }

            // Map the DTO to the Event entity
            var eventEntity = _mapper.Map<Event>(eventDto);
            eventEntity.Organizer = organizer;
            
            await _unitOfWork.Events.AddAsync(eventEntity);
            await _unitOfWork.CompleteAsync();
            return Ok(new { message = "Event added successfully" });
            
        }

        [HttpPut("updateEventTag")]
        public async Task<IActionResult> UpdateEventTag(int eventId, string tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var eventEntity = await _eventRepository.GetEventById(eventId);

            if(eventEntity == null)
            {
                return NotFound("Event not found");
            }
            eventEntity.Hashtag = tag;
            _unitOfWork.Events.Update(eventEntity);
            await _unitOfWork.CompleteAsync();

            return Ok("Changed Event Tag");

           
        }

        [HttpPut("updateEventStatus")]
        public async Task<IActionResult> UpdateEventStatus(int eventId, string status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var eventEntity = await _eventRepository.GetEventById(eventId);

            if (eventEntity == null)
            {
                return NotFound("Event not found");
            }

            //Draft, OpenForBooking, Cancelled, Closed

            eventEntity.EventStatus = status;
            _unitOfWork.Events.Update(eventEntity);
            await _unitOfWork.CompleteAsync();

            return Ok("Changed Event Status");
        }
    }
}
