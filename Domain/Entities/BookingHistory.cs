using ManagementSystem.Api.Common.Domain;

namespace ManagementSystem.Api.Domain.Entities;

public class BookingHistory : BaseEntity
{
    private BookingHistory()
    {
    }

    public BookingHistory(Booking booking, string description)
    {
        BookingId = booking.Id;
        Booking = booking;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid BookingId { get; private set; }
    public Booking Booking { get; private set; } = null!;
    public string Description { get; private set; } = null!;
}