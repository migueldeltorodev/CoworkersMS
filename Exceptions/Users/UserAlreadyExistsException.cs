using System.Net;
using ManagementSystem.Api.Common.Exceptions;

namespace ManagementSystem.Api.Exceptions.Users;

public class UserAlreadyExistsException : BaseException
{
    public UserAlreadyExistsException(string message = "User already exists.")
        : base(message, HttpStatusCode.Conflict)
    {
    }
}