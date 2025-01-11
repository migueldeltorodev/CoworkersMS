using ManagementSystem.Api.Common.Results;
using MediatR;

namespace ManagementSystem.Api.Features.Bookings.Commands.CancelBooking;

public record CancelBookingCommand : IRequest<Result<Unit>>
{
    public Guid BookingId { get; init; }
    public Guid UserId { get; init; }
    public string Reason { get; init; } = null!;
}