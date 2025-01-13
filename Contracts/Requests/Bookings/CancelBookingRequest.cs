namespace ManagementSystem.Api.Contracts.Requests.Bookings;

public record CancelBookingRequest
{
    public string Reason { get; init; } = null!;
}