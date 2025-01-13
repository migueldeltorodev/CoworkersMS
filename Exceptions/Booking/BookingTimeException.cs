using System.Net;
using ManagementSystem.Api.Common.Exceptions;

namespace ManagementSystem.Api.Exceptions.Booking;

public class BookingTimeException : BaseException
{
    public BookingTimeException(string message,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError) 
        : base(message, statusCode)
    {
    }
}