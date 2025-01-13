namespace ManagementSystem.Api.Common.Authentication;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string Secret { get; init; } = default!;
    public int ExpiryInMinutes { get; init; }
    public string Issuer { get; init; } = default!;
    public string Audience { get; init; } = default!;
}