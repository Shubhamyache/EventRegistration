using EventRegistrationWebAPI.Data;
using EventRegistrationWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace EventRegistrationWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CsvController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CsvController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("export-EventRegistrationCount-csv")]
        public async Task<IActionResult> GetEventRegistrationsCount() // detailed information about event registrations, including event details, organizer information, and ticket counts by ticket category
        {
            var data = await _context.Events.Include(e => e.Registrations).Include(e => e.Organizer).ToListAsync();
            var csv = new StringBuilder();
            csv.AppendLine("EventName,Category,EventDate,MinimumAge,OrganizerFirstName,OrganizerLastName,PlatinumTicketsCount,GoldTicketsCount,SilverTicketsCount");

            foreach (var e in data)
            {
                csv.AppendLine($"{e.EventName},{e.Category},{e.EventStartDateTime.ToShortDateString()},{e.MinimumAge},{e.Organizer.FirstName},{e.Organizer.LastName},{e.Registrations.Sum(r => r.PlatinumTicketsCount)},{e.Registrations.Sum(r => r.GoldTicketsCount)},{e.Registrations.Sum(r => r.SilverTicketsCount)}");
            }

            var filename = "EventRegistrationsTicketCategory.csv";
            var byteArray = Encoding.UTF8.GetBytes(csv.ToString());
            var stream = new MemoryStream(byteArray);
            return File(stream, "text/csv", filename);
        }


        [HttpGet("export-UserRegistrationCount-csv")]
        public async Task<IActionResult> GetUserRegistrationsList()
        {
            var data = await _context.Users.Include(user => user.Registrations).ThenInclude(r => r.Event).ToListAsync();
            var csv = new StringBuilder();
            csv.AppendLine("userFirstName,userLastName,userEmail,EventCount,PastEventCount,FutureEventCount");

            foreach (var u in data)
            {
                csv.AppendLine($"{u.FirstName},{u.LastName},{u.Email},{u.Registrations.Count},{u.Registrations.Where(r => r.Event.EventStartDateTime <= DateTime.Now).Count()},{u.Registrations.Where(r => r.Event.EventStartDateTime > DateTime.Now).Count()}");
            }

            var filename = "UserRegistrations.csv";
            var byteArray = Encoding.UTF8.GetBytes(csv.ToString());
            var stream = new MemoryStream(byteArray);
            return File(stream, "text/csv", filename);
        }

        [HttpGet("export-MovementCountPerday-csv")]
        public async Task<IActionResult> GetMovementCount()
        {
            var data = _context.Registrations.Where(r => r.RegistrationStatus == "Completed")
                .GroupBy(r => new { r.EventId, r.Event.EventName, Date = r.ApproveDate })
                .Select(g => new RegistrationStats
                {
                    EventName = g.Key.EventName,
                    EventId = g.Key.EventId,
                   // ApprovalDate = g.Key.Date,
                    NumberOfApprovals = g.Count()
                });
            var csv = new StringBuilder();
            csv.AppendLine("EventId,EventName,ApprovalDate,NumberOfApprovals");

            foreach (var e in data)
            {
                csv.AppendLine($"{e.EventId},{e.EventName},{e.ApprovalDate.ToShortDateString()},{e.NumberOfApprovals}");
            }

            var filename = "MovementCountPerday.csv";
            var byteArray = Encoding.UTF8.GetBytes(csv.ToString());
            var stream = new MemoryStream(byteArray);
            return File(stream, "text/csv", filename);
        }

        [HttpGet("export-CitywiseEventList-csv")]
        public async Task<IActionResult> GetCityWiseEventList()
        {
            var data = await _context.Events.Where(e => e.MinimumAge > 5 && e.EventStatus == "OpenForBooking").Include(e => e.Venue).ToListAsync();
            var csv = new StringBuilder();
            csv.AppendLine("City,EventName,Category,EventStartDateTime,EventEndDateTime,RegistrationCloseDate,MinimumAge,EventStatus,Hashtag,Description");

            foreach (var e in data)
            {
                csv.AppendLine($"{e.Venue.City},{e.EventName},{e.Category}, {e.EventStartDateTime.ToShortDateString()},{e.EventEndDateTime.ToShortDateString()}, {e.RegistrationCloseDate.ToShortDateString()},{e.MinimumAge},{e.EventStatus}, {e.Hashtag}, {e.Description}");
            }

            var filename = "CitywiseEvents.csv";
            var byteArray = Encoding.UTF8.GetBytes(csv.ToString());
            var stream = new MemoryStream(byteArray);
            return File(stream, "text/csv", filename);
        }
    }
    public class RegistrationStats
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime ApprovalDate { get; set; }
        public int NumberOfApprovals { get; set; }
    }
}