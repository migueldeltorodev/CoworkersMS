using ManagementSystem.Api.Contracts.DTOs;
using ManagementSystem.Api.Contracts.Requests.Bookings;
using ManagementSystem.Api.Contracts.Responses;
using ManagementSystem.Api.Features.Bookings.Commands.CancelBooking;
using ManagementSystem.Api.Features.Bookings.Commands.CreateBooking;

namespace ManagementSystem.Api.Mappings.Booking;

public static class BookingContractExtensions
{
    public static CreateBookingCommand ToCommand(this CreateBookingRequest request, Guid userId)
        => new()
        {
            UserId = userId,
            RoomId = request.RoomId,
            StartTime = request.StartTime,
            EndTime = request.EndTime
        };

    public static CancelBookingCommand ToCommand(this CancelBookingRequest request, Guid bookingId, Guid userId)
        => new()
        {
            BookingId = bookingId,
            UserId = userId,
            Reason = request.Reason
        };

    public static BookingResponse ToResponse(this BookingDto booking)
        => new()
        {
            Id = booking.Id,
            UserId = booking.UserId,
            RoomId = booking.RoomId,
            RoomName = booking.RoomName,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status.ToString(),
            TotalAmount = booking.TotalAmount
        };
}