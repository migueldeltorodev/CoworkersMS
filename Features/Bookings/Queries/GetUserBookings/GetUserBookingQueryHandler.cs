using AutoMapper;
using ManagementSystem.Api.Common.Interfaces;
using ManagementSystem.Api.Common.Results;
using ManagementSystem.Api.Contracts.DTOs;
using MediatR;

namespace ManagementSystem.Api.Features.Bookings.Queries.GetUserBookings;

public class GetUserBookingsQueryHandler : IRequestHandler<GetUserBookingsQuery, Result<List<BookingDto>>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public GetUserBookingsQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<BookingDto>>> Handle(
        GetUserBookingsQuery request,
        CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetUserBookingsAsync(
            request.UserId, request.FromDate, cancellationToken);

        return Result<List<BookingDto>>.Success(_mapper.Map<List<BookingDto>>(bookings));
    }
}