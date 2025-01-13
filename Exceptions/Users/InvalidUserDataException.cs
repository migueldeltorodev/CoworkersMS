using System.Net;
using ManagementSystem.Api.Common.Exceptions;

namespace ManagementSystem.Api.Exceptions.Users;

public class InvalidUserDataException : BaseException
{
    public InvalidUserDataException(string message = "Invalid user data.")
        : base(message, HttpStatusCode.BadRequest)
    {
    }
}