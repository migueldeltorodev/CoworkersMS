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

        if (minCapacity.HasValue) query = query.Where(r => r.Capacity >= minCapacity.Value);

        if (!string.IsNullOrWhiteSpace(location))
            query = query.Where(r => r.Location.ToLower().Contains(location.ToLower()));

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken)
    {
        return await _dbSet.AnyAsync(r => r.Name == name, cancellationToken);
    }

    public async Task<IReadOnlyList<Room>> GetRoomsAsync(
        string? searchTerm,
        string? location,
        int? minCapacity,
        decimal? maxHourlyRate,
        bool? isActive,
        CancellationToken cancellationToken)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(r =>
                r.Name.ToLower().Contains(searchTerm) ||
                (r.Description != null && r.Description.ToLower().Contains(searchTerm)));
        }

        if (!string.IsNullOrWhiteSpace(location)) query = query.Where(r => r.Location == location);

        if (minCapacity.HasValue) query = query.Where(r => r.Capacity >= minCapacity.Value);

        if (maxHourlyRate.HasValue) query = query.Where(r => r.HourlyRate <= maxHourlyRate.Value);

        if (isActive.HasValue) query = query.Where(r => r.IsActive == isActive.Value);

        return await query
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }
}