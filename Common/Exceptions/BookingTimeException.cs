using System.Net;

namespace ManagementSystem.Api.Common.Exceptions;

public class BookingTimeException : BaseException
{
    public BookingTimeException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, statusCode)
    {
    }
}