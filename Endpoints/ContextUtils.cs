using System.Security.Claims;

namespace ManagementSystem.Api.Endpoints;

public static class ContextUtils
{
    public static Guid GetUserIdFromContext(this HttpContext context)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? throw new UnauthorizedAccessException("User ID not found in token");
        
        return Guid.Parse(userIdClaim);
    }
}