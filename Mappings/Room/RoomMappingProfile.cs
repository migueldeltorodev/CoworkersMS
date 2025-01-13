using AutoMapper;
using ManagementSystem.Api.Contracts.Responses;

namespace ManagementSystem.Api.Mappings.Room;

public class RoomMappingProfile : Profile
{
    public RoomMappingProfile()
    {
        CreateMap<Domain.Entities.Room, RoomResponse>();
    }
}