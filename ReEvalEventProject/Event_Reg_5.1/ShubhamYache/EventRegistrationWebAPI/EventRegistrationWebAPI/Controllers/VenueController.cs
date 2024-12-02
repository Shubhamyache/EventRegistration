using AutoMapper;
using EventRegistrationWebAPI.DTOs.VenueDto;
using EventRegistrationWebAPI.Models;
using EventRegistrationWebAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventRegistrationWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenueController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public VenueController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetVenues")]
        public async Task<ActionResult<IEnumerable<VenueDto>>> GetVenues()
        {
            var venues = await _unitOfWork.Venues.GetAllAsync();
            var result = _mapper.Map<IEnumerable<VenueDto>>(venues);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetVenueById")]
        public async Task<ActionResult<VenueDto>> GetVenueById(int id)
        {
            var venue = await _unitOfWork.Venues.GetByIdAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<VenueDto>(venue);
            return Ok(result);
        }

        [HttpPost]
        [Route("AddVenue")]
        public async Task<ActionResult> AddVenue(CreateVenueDto venueDto)
        {
            var venue = _mapper.Map<Venue>(venueDto);
            await _unitOfWork.Venues.AddAsync(venue);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpPut]
        [Route("UpdateVenue")]
        public async Task<ActionResult> UpdateVenue(VenueDto venueDto)
        {
            var venue = _mapper.Map<Venue>(venueDto);
            _unitOfWork.Venues.Update(venue);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteVenue")]
        public async Task<ActionResult> DeleteVenue(int id)
        {
            var venue = await _unitOfWork.Venues.GetByIdAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            _unitOfWork.Venues.Delete(venue);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }
    }
}
