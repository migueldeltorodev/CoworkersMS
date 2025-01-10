using ManagementSystem.Api.Common.Domain;

namespace ManagementSystem.Api.Domain.Entities;

public class Room : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Location { get; private set; } = null!;
    public int Capacity { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public decimal HourlyRate { get; private set; }
    
    private readonly List<Booking> _bookings = new();
    public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();
    
    private Room() { }
    
    public Room(string name, string location, int capacity, decimal hourlyRate, string? description = null)
    {
        Name = name;
        Location = location;
        Capacity = capacity;
        HourlyRate = hourlyRate;
        Description = description;
        IsActive = true;
    }
    
    public void Update(string name, string location, int capacity, decimal hourlyRate, string? description)
    {
        Name = name;
        Location = location;
        Capacity = capacity;
        HourlyRate = hourlyRate;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}