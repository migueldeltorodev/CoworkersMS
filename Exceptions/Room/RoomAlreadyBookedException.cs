using System.Net;
using ManagementSystem.Api.Common.Exceptions;

namespace ManagementSystem.Api.Exceptions.Room;

public class RoomAlreadyBookedException : BaseException
{
    public RoomAlreadyBookedException(string message = "Room is already booked.")
        : base(message, HttpStatusCode.Conflict)
    {
    }
}