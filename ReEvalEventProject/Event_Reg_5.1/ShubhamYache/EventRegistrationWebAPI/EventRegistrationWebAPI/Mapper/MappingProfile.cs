using AutoMapper;
using EventRegistrationWebAPI.DTOs.EventDto;
using EventRegistrationWebAPI.DTOs;
using EventRegistrationWebAPI.DTOs.VenueDto;
using EventRegistrationWebAPI.Models;
using EventRegistrationWebAPI.DTOs.RegistrationDto;
using EventRegistrationWebAPI.DTOs.UserDto;
using EventRegistrationWebAPI.DTOs.Organizer;

namespace EventRegistrationWebAPI.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Event Mappings
            CreateMap<Event, EventDto>().ReverseMap();
            CreateMap<Event, CreateEventDto>().ReverseMap();

            CreateMap<Event, PostEventDto>()
               .ForMember(dest => dest.Organizer, opt => opt.MapFrom(src => new OrganizerDto
               {
                   FirstName = src.Organizer.FirstName,
                   LastName = src.Organizer.LastName,
                   Email = src.Organizer.Email
               }))
               .ForMember(dest => dest.Venue, opt => opt.MapFrom(src => src.Venue))
               .ForMember(dest => dest.PlatinumTicketsPrice, opt => opt.MapFrom(src => src.PlatinumTicketsPrice))
               .ForMember(dest => dest.GoldTicketsPrice, opt => opt.MapFrom(src => src.GoldTicketsPrice))
               .ForMember(dest => dest.SilverTicketsPrice, opt => opt.MapFrom(src => src.SilverTicketsPrice));



            // Venue Mappings
            CreateMap<Venue, VenueDto>().ReverseMap();

            // Registration Mappings
            CreateMap<Registration, RegistrationDto>()
             .ForMember(dest => dest.PaymentDto, opt => opt.MapFrom(src => src.Payment)); // Ensure Payments map to PaymentDto

            //Create Registration Mappings
            CreateMap<Registration, CreateRegistrationDto>().ReverseMap();

            // Payment Mappings
            CreateMap<Payment, PaymentDto>().ReverseMap();
            CreateMap<Payment, CreatePaymentDto>().ReverseMap();

            // User Mappings
            CreateMap<ApplicationUser, UserDto>().ReverseMap();

            CreateMap<Registration, SimplifiedRegistrationDto>()
             .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.EventName)) // Mapping Event
             .ForMember(dest => dest.PaymentAmount, opt => opt.MapFrom(src => src.Payment != null ? src.Payment.PaymentAmount : 0))
             .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.Payment != null ? src.Payment.PaymentMethod : null))
             .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.Payment != null ? src.Payment.PaymentStatus : null));

            // Venue Mappings
            CreateMap<Venue, VenueDto>().ReverseMap();
            CreateMap<Venue, CreateVenueDto>().ReverseMap();

        }
    }

}
