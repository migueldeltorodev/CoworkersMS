using ManagementSystem.Api.Common.Results;
using MediatR;

namespace ManagementSystem.Api.Features.Rooms.Commands.CreateRoom;

public record CreateRoomCommand : IRequest<Result<Guid>>
{
    public string Name { get; init; } = null!;
    public string Location { get; init; } = null!;
    public int Capacity { get; init; }
    public decimal HourlyRate { get; init; }
    public string? Description { get; init; }
}