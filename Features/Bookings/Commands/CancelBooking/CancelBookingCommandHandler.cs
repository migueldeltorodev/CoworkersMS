using ManagementSystem.Api.Common.Exceptions;
using ManagementSystem.Api.Common.Interfaces;
using ManagementSystem.Api.Common.Results;
using ManagementSystem.Api.Domain.Entities;
using MediatR;

namespace ManagementSystem.Api.Features.Bookings.Commands.CancelBooking;

public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, Result<Unit>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelBookingCommandHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken)
                      ?? throw new NotFoundException(nameof(Booking), request.BookingId);

        if (booking.UserId != request.UserId)
        {
            return Result<Unit>.Failure("You can only cancel your own bookings");
        }

        booking.Cancel(request.Reason);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}