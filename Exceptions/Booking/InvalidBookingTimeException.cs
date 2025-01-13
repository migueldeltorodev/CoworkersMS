using System.Net;
using ManagementSystem.Api.Common.Exceptions;

namespace ManagementSystem.Api.Exceptions.Booking;

public class InvalidBookingTimeException : BaseException
{
    public InvalidBookingTimeException(string message = "Invalid booking time.")
        : base(message, HttpStatusCode.BadRequest)
    {
    }
}