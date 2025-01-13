using ManagementSystem.Api.Common.Interfaces;
using ManagementSystem.Api.Database;
using ManagementSystem.Api.Domain.Entities;
using ManagementSystem.Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Api.Persistence.Repositories;

public class BookingRepository : BaseRepository<Booking>, IBookingRepository
{
    public BookingRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Booking>> GetUserBookingsAsync(
        Guid userId,
        DateTime? fromDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(b => b.Room)
            .Where(b => b.UserId == userId);

        if (fromDate.HasValue)
        {
            query = query.Where(b => b.StartTime >= fromDate.Value);
        }

        return await query
            .OrderByDescending(b => b.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasOverlappingBookingAsync(
        Guid roomId,
        DateTime startTime,
        DateTime endTime,
        Guid? excludeBookingId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Where(b => b.RoomId == roomId && b.Status != BookingStatus.Cancelled)
            .Where(b =>
                (b.StartTime <= startTime && b.EndTime > startTime) ||
                (b.StartTime < endTime && b.EndTime >= endTime) ||
                (b.StartTime >= startTime && b.EndTime <= endTime));

        if (excludeBookingId.HasValue)
        {
            query = query.Where(b => b.Id != excludeBookingId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }
}