using ManagementSystem.Api.Domain.Entities;

namespace ManagementSystem.Api.Common.Interfaces;

public interface IRoomRepository : IRepository<Room>
{
    Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(
        DateTime startTime,
        DateTime endTime,
        int? minCapacity = null,
        string? location = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
    Task<IReadOnlyList<Room>> GetRoomsAsync(
        string? searchTerm,
        string? location,
        int? minCapacity,
        decimal? maxHourlyRate,
        bool? isActive,
        CancellationToken cancellationToken);
}