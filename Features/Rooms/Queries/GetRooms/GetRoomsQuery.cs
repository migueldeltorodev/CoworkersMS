using ManagementSystem.Api.Common.Results;
using ManagementSystem.Api.Contracts.Responses;
using MediatR;

namespace ManagementSystem.Api.Features.Rooms.Queries.GetRooms;

public record GetRoomsQuery : IRequest<Result<IReadOnlyList<RoomResponse>>>
{
    public string? SearchTerm { get; init; }
    public string? Location { get; init; }
    public int? MinCapacity { get; init; }
    public decimal? MaxHourlyRate { get; init; }
    public bool? IsActive { get; init; }
}