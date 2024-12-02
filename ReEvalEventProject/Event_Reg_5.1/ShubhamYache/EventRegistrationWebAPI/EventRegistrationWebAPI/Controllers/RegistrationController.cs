using AutoMapper;
using EventRegistrationWebAPI.DTOs;
using EventRegistrationWebAPI.DTOs.RegistrationDto;
using EventRegistrationWebAPI.DTOs.RegistrationModel;
using EventRegistrationWebAPI.Models;
using EventRegistrationWebAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace EventRegistrationWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RegistrationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

       // [Authorize(Roles = "Organizer")]
        [HttpGet]
        [Route("GetRegistrations")]
        public async Task<ActionResult<IEnumerable<RegistrationDto>>> GetRegistrations()
        {
            var registrations = await _unitOfWork.Registrations.GetAllRegistrations();
            var result = _mapper.Map<IEnumerable<RegistrationDto>>(registrations);
            return Ok(result);
        }

        //[Authorize(Roles = "Organizer")]
        [HttpGet]
        [Route("GetRegistrationsByUserId")]
        public async Task<ActionResult<IEnumerable<RegistrationDto>>> GetRegistrationsByUserId(string userId)
        {
            var registrations = await _unitOfWork.Registrations.GetRegistrationsByUserIdAsync(userId);
            var registrationDto = _mapper.Map<IEnumerable<RegistrationDto>>(registrations);
            return Ok(registrationDto);
        }

        //[Authorize(Roles = "User")]
        [HttpPost]
        [Route("AddRegistration")]
        public async Task<ActionResult> AddRegistration(CreateRegistrationDto registrationDto)
        {
            var eventEntity = await _unitOfWork.Events.GetByIdAsync(registrationDto.EventId);
            string OverSubscribedMessage = string.Empty;
            if (eventEntity == null)
            {
                return NotFound("Event not found"); 
            }

            var registrations = await _unitOfWork.Registrations.GetRegistrationsByEventId(registrationDto.EventId);

            var totalPlatinumTicketsRegistered = registrations.Where(r=>r.EventId==registrationDto.EventId).Sum(r => r.PlatinumTicketsCount);
            var totalGoldTicketsRegistered = registrations.Where(r => r.EventId == registrationDto.EventId).Sum(r => r.GoldTicketsCount);
            var totalSilverTicketsRegistered = registrations.Where(r => r.EventId == registrationDto.EventId).Sum(r => r.SilverTicketsCount);

            Console.WriteLine(totalPlatinumTicketsRegistered+' '+totalGoldTicketsRegistered+' '+totalSilverTicketsRegistered);


            //Oversubscription logic with respect to the event
            if (registrationDto.PlatinumTicketsCount + totalPlatinumTicketsRegistered > eventEntity.PlatinumTicketsNumber)
            {
                int oversubscribedTickets = (registrationDto.PlatinumTicketsCount + totalPlatinumTicketsRegistered) - eventEntity.PlatinumTicketsNumber;
                eventEntity.PlatinumTicketsOversubscribed += oversubscribedTickets;  // Increment oversubscription count
                OverSubscribedMessage = "Platinum Tickets are oversubscribed, ";
            }

            if (registrationDto.GoldTicketsCount + totalGoldTicketsRegistered > eventEntity.GoldTicketsNumber)
            {
                int oversubscribedTickets = (registrationDto.GoldTicketsCount + totalGoldTicketsRegistered) - eventEntity.GoldTicketsNumber;
                eventEntity.GoldTicketsOversubscribed += oversubscribedTickets;  // Increment oversubscription count
                OverSubscribedMessage += "Gold Tickets are oversubscribed, ";
            }

            if (registrationDto.SilverTicketsCount + totalSilverTicketsRegistered > eventEntity.SilverTicketsNumber)
            {
                int oversubscribedTickets = (registrationDto.SilverTicketsCount + totalSilverTicketsRegistered) - eventEntity.SilverTicketsNumber;
                eventEntity.SilverTicketsOversubscribed += oversubscribedTickets;  // Increment oversubscription count
                OverSubscribedMessage += "Silver Tickets are oversubscribed";
            }
            

            // Save oversubscription changes to the event
            _unitOfWork.Events.Update(eventEntity);
            await _unitOfWork.CompleteAsync();

            if (OverSubscribedMessage != string.Empty)
            {
                return BadRequest(OverSubscribedMessage);
            }

            // Proceed with adding registration and payments
            var registration = _mapper.Map<Registration>(registrationDto);

            var payments = _mapper.Map<Payment>(registrationDto.PaymentDto);

            //Logic to check if the payment amount matches the ticket prices
            var eventDetails = await _unitOfWork.Events.GetByIdAsync(registrationDto.EventId);
            var platinumPrice = eventDetails.PlatinumTicketsPrice;
            var goldPrice = eventDetails.GoldTicketsPrice;
            var silverPrice = eventDetails.SilverTicketsPrice;
            if(payments.PaymentAmount != platinumPrice * registrationDto.PlatinumTicketsCount + goldPrice * registrationDto.GoldTicketsCount + silverPrice * registrationDto.SilverTicketsCount)
            {
                return BadRequest("Payment amount does not match the ticket prices. Please try again.");
            }

            var registrationData = await _unitOfWork.Registrations.AddRegistration(registration, payments);
            await _unitOfWork.CompleteAsync();

            var result = _mapper.Map<SimplifiedRegistrationDto>(registrationData);
            return Ok("Registration Successfull!");
        }

        [HttpPut]
        [Route("CancelRegistration")]
        public async Task<ActionResult<IEnumerable<RegistrationDto>>> CancelRegistration([FromQuery]int id)
        {

           
            var registration = await _unitOfWork.Registrations.GetByIdAsync(id);
            var cancelledRegistration = await _unitOfWork.Registrations.CancelRegistrationAsync(id, registration);

            return Ok(cancelledRegistration);


/*            if(cancelledRegistration.RegistrationStatus == "Cancelled")
            {
                foreach(var payment in cancelledRegistration.Payments)
                {
                    if(payment.PaymentStatus != "Refund Inialized")
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status="failure",  Message ="Could not complete cancellation."});
                    }
                    return Ok();
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "failure", Message = "Could not complete cancellation." });*/
        }
    }
}
