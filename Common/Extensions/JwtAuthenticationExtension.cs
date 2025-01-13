using System.Text;
using ManagementSystem.Api.Common.Authentication;
using ManagementSystem.Api.Common.Interfaces;
using ManagementSystem.Api.Domain.Entities;
using ManagementSystem.Api.Persistence.JWT;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace ManagementSystem.Api.Common.Extensions;

public static class JwtAuthenticationExtension
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JWT_OR_COOKIE";
                options.DefaultChallengeScheme = "JWT_OR_COOKIE";
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "X-Access-Token";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
            })
            .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    string authorization = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                        return JwtBearerDefaults.AuthenticationScheme;

                    return CookieAuthenticationDefaults.AuthenticationScheme;
                };
            });

        services.AddAuthorization();

        return services;
    }
}