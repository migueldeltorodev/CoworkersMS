using ManagementSystem.Api.Common.Results;
using ManagementSystem.Api.Contracts.DTOs;
using MediatR;

namespace ManagementSystem.Api.Features.Bookings.Queries.GetUserBookings;

public record GetUserBookingsQuery : IRequest<Result<List<BookingDto>>>
{
    public Guid UserId { get; init; }
    public DateTime? FromDate { get; init; }
}