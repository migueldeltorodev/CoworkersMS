using AutoMapper;
using ManagementSystem.Api.Contracts.DTOs;

namespace ManagementSystem.Api.Mappings.Booking;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<Domain.Entities.Booking, BookingDto>()
            .ForMember(d => d.RoomName, opt => opt.MapFrom(s => s.Room.Name));
    }
}