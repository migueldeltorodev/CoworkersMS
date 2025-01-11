using ManagementSystem.Api.Common.Domain;
using ManagementSystem.Api.Common.Exceptions;
using ManagementSystem.Api.Domain.Enums;

namespace ManagementSystem.Api.Domain.Entities;

public class Booking : BaseEntity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    
    public Guid RoomId { get; private set; }
    public Room Room { get; private set; } = null!;
    
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public BookingStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    
    private readonly List<BookingHistory> _history = new();
    public IReadOnlyCollection<BookingHistory> History => _history.AsReadOnly();
    
    private Booking() { } // For EF Core
    
    public Booking(User user, Room room, DateTime startTime, DateTime endTime)
    {
        if (startTime >= endTime)
            throw new DomainException("Start time must be before end time");
            
        if (startTime < DateTime.UtcNow)
            throw new DomainException("Cannot create bookings in the past");
            
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
    
    private decimal CalculateTotalAmount(decimal hourlyRate, DateTime startTime, DateTime endTime)
    {
        var duration = endTime - startTime;
        var hours = Math.Ceiling(duration.TotalHours);
        return hourlyRate * (decimal)hours;
    }
    
    public void Cancel(string reason)
    {
        if (Status != BookingStatus.Created && Status != BookingStatus.Confirmed)
            throw new DomainException("Cannot cancel a booking that is not active");
            
        if (StartTime <= DateTime.UtcNow)
            throw new DomainException("Cannot cancel a booking that has already started");
            
        Status = BookingStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
        AddHistoryEntry($"Booking cancelled: {reason}");
    }
    
    public void Confirm()
    {
        if (Status != BookingStatus.Created)
            throw new DomainException("Can only confirm bookings in Created status");
            
        Status = BookingStatus.Confirmed;
        UpdatedAt = DateTime.UtcNow;
        AddHistoryEntry("Booking confirmed");
    }
    
    private void AddHistoryEntry(string description)
    {
        _history.Add(new BookingHistory(this, description));
    }
}