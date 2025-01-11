namespace ManagementSystem.Api.Common.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }   
}