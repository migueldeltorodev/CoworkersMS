using ManagementSystem.Api.Common.Domain;
using ManagementSystem.Api.Domain.Enums;
using ManagementSystem.Api.Exceptions.Booking;

namespace ManagementSystem.Api.Domain.Entities;

public class Booking : BaseEntity, IHasConcurrencyToken
{
    private readonly List<BookingHistory> _history = new();

    private Booking()
    {
    }

    public Booking(User user, Room room, DateTime startTime, DateTime endTime)
    {
        if (startTime >= endTime)
            throw new BookingTimeException("Start time must be before end time");

        if (startTime < DateTime.UtcNow)
            throw new BookingTimeException("Cannot create bookings in the past");

        UserId = user.Id;
        User = user;
        RoomId = room.Id;
        Room = room;
        StartTime = startTime;
        EndTime = endTime;
        Status = BookingStatus.Created;
        TotalAmount = CalculateTotalAmount(room.HourlyRate, startTime, endTime);

        AddHistoryEntry("Booking created");
    }

    public Guid UserId { get; private set; }
    public User User { get; private set; } = default!;

    public Guid RoomId { get; private set; }
    public Room Room { get; private set; } = default!;

    public DateTime StartTime { get; }
    public DateTime EndTime { get; private set; }
    public BookingStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public IReadOnlyCollection<BookingHistory> History => _history.AsReadOnly();

    private decimal CalculateTotalAmount(decimal hourlyRate, DateTime startTime, DateTime endTime)
    {
        var duration = endTime - startTime;
        var hours = Math.Ceiling(duration.TotalHours);
        return hourlyRate * (decimal)hours;
    }

    public void Cancel(string reason)
    {
        if (Status != BookingStatus.Created && Status != BookingStatus.Confirmed)
            throw new BookingTimeException("Cannot cancel a booking that is not active");

        if (StartTime <= DateTime.UtcNow)
            throw new BookingTimeException("Cannot cancel a booking that has already started");

        Status = BookingStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
        AddHistoryEntry($"Booking cancelled: {reason}");
    }

    // TODO: Implement endpoints for this
    public void Confirm()
    {
        if (Status != BookingStatus.Created)
            throw new BookingTimeException("Can only confirm bookings in Created status");

        Status = BookingStatus.Confirmed;
        UpdatedAt = DateTime.UtcNow;
        AddHistoryEntry("Booking confirmed");
    }

    private void AddHistoryEntry(string description)
    {
        _history.Add(new BookingHistory(this, description));
    }

    public byte[] RowVersion { get; set; } = default!;
}