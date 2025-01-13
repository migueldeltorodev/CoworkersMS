using ManagementSystem.Api.Common.Interfaces;
using ManagementSystem.Api.Database;
using ManagementSystem.Api.Domain.Entities;
using ManagementSystem.Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Api.Persistence.Repositories;

public class RoomRepository : BaseRepository<Room>, IRoomRepository
{
    public RoomRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(
        DateTime startTime,
        DateTime endTime,
        int? minCapacity = null,
        string? location = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Where(r => r.IsActive)
            .Where(r => !r.Bookings.Any(b =>
                b.Status != BookingStatus.Cancelled &&
                ((b.StartTime <= startTime && b.EndTime > startTime) ||
                 (b.StartTime < endTime && b.EndTime >= endTime) ||
                 (b.StartTime >= startTime && b.EndTime <= endTime))));

        if (minCapacity.HasValue)
        {
            query = query.Where(r => r.Capacity >= minCapacity.Value);
        }

        if (!string.IsNullOrWhiteSpace(location))
        {
            query = query.Where(r => r.Location.ToLower().Contains(location.ToLower()));
        }

        return await query.ToListAsync(cancellationToken);
    }
}