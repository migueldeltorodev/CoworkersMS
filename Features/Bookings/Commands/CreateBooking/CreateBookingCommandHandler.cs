using ManagementSystem.Api.Common.Exceptions;
using ManagementSystem.Api.Common.Interfaces;
using ManagementSystem.Api.Common.Results;
using ManagementSystem.Api.Domain.Entities;
using MediatR;

namespace ManagementSystem.Api.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Result<Guid>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookingCommandHandler(
        IBookingRepository bookingRepository,
        IUserRepository userRepository,
        IRoomRepository roomRepository,
        IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _userRepository = userRepository;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
                   ?? throw new NotFoundException(nameof(User), request.UserId);

        var room = await _roomRepository.GetByIdAsync(request.RoomId, cancellationToken)
                   ?? throw new NotFoundException(nameof(Room), request.RoomId);

        var hasOverlap = await _bookingRepository.HasOverlappingBookingAsync(
            request.RoomId, request.StartTime, request.EndTime, null, cancellationToken);

        if (hasOverlap)
        {
            return Result<Guid>.Failure("Room is already booked for the selected time period");
        }

        var booking = new Booking(user, room, request.StartTime, request.EndTime);
        await _bookingRepository.AddAsync(booking, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(booking.Id);
    }
}