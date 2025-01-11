using ManagementSystem.Api.Common.Results;
using MediatR;

namespace ManagementSystem.Api.Features.Bookings.Commands.CreateBooking;

public record CreateBookingCommand : IRequest<Result<Guid>>
{
    public Guid UserId { get; init; }
    public Guid RoomId { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
}