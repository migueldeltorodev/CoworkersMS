using ManagementSystem.Api.Contracts.Requests.Rooms;
using ManagementSystem.Api.Features.Rooms.Commands.CreateRoom;
using ManagementSystem.Api.Features.Rooms.Queries.GetRooms;

namespace ManagementSystem.Api.Mappings.Room;

public static class RoomMappingExtensions
{
    public static CreateRoomCommand ToCommand(this CreateRoomRequest request)
    {
        return new CreateRoomCommand
        {
            Name = request.Name,
            Location = request.Location,
            Capacity = request.Capacity,
            HourlyRate = request.HourlyRate,
            Description = request.Description
        };
    }
    
    public static GetRoomsQuery ToQuery(this GetRoomsRequest request)
    {
        return new GetRoomsQuery
        {
            SearchTerm = request.SearchTerm,
            Location = request.Location,
            MinCapacity = request.MinCapacity,
            MaxHourlyRate = request.MaxHourlyRate,
            IsActive = request.IsActive
        };
    }
}