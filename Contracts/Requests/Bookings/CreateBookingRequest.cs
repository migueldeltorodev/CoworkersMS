namespace ManagementSystem.Api.Contracts.Requests.Bookings;

public record CreateBookingRequest
{
    public Guid RoomId { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
}