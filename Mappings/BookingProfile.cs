using AutoMapper;
using ManagementSystem.Api.Contracts.DTOs;
using ManagementSystem.Api.Domain.Entities;

namespace ManagementSystem.Api.Mappings;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<Booking, BookingDto>()
            .ForMember(d => d.RoomName, opt => opt.MapFrom(s => s.Room.Name));
    }
}