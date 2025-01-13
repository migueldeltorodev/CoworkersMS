using System.Net;

namespace ManagementSystem.Api.Common.Exceptions;

public abstract class BaseException : Exception
{
    protected BaseException(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }
}