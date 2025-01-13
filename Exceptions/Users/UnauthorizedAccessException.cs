using System.Net;
using ManagementSystem.Api.Common.Exceptions;

namespace ManagementSystem.Api.Exceptions.Users;

public class UnauthorizedAccessException : BaseException
{
    public UnauthorizedAccessException(string message = "Unauthorized access.")
        : base(message, HttpStatusCode.Unauthorized)
    {
    }
}