using ManagementSystem.Api.Common.Domain;
using ManagementSystem.Api.Domain.Enums;

namespace ManagementSystem.Api.Domain.Entities;

public class User : BaseEntity
{
    private readonly List<Booking> _bookings = new();

    private User()
    {
    }

    public User(string email, string firstName, string lastName, string passwordHash)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PasswordHash = passwordHash;
        Role = UserRole.Standard;
        IsActive = true;
    }

    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();

    public void UpdatePersonalInfo(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetRole(UserRole role)
    {
        Role = role;
        UpdatedAt = DateTime.UtcNow;
    }
}