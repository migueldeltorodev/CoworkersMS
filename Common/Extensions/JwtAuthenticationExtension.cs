using System.Text;
using ManagementSystem.Api.Common.Authentication;
using ManagementSystem.Api.Common.Interfaces;
using ManagementSystem.Api.Domain.Entities;
using ManagementSystem.Api.Persistence.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()  
            {  
                ValidateIssuer = true,  
                ValidateAudience = true,  
                ValidateLifetime = true,  
                ValidateIssuerSigningKey = true,  
                ValidIssuer = jwtSettings.Issuer,  
                ValidAudience = jwtSettings.Audience,  
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))  
            });

        services.AddAuthorization();
        
        return services;  
    }
}