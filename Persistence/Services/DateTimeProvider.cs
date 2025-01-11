using ManagementSystem.Api.Common.Interfaces;

namespace ManagementSystem.Api.Persistence.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}