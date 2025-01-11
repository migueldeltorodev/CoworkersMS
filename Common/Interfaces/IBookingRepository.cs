using ManagementSystem.Api.Domain.Entities;

namespace ManagementSystem.Api.Common.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IReadOnlyList<Booking>> GetUserBookingsAsync(
        Guid userId,
        DateTime? fromDate = null,
        CancellationToken cancellationToken = default);
        
    Task<bool> HasOverlappingBookingAsync(
        Guid roomId,
        DateTime startTime,
        DateTime endTime,
        Guid? excludeBookingId = null,
        CancellationToken cancellationToken = default);
}