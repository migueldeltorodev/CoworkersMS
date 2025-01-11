using System.Text;
using ManagementSystem.Api.Common.Authentication;
using ManagementSystem.Api.Common.Interfaces;
using ManagementSystem.Api.Persistence.Identity.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ManagementSystem.Api.Common.Extensions;

public static class JwtExtension
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);
        
        services.AddSingleton(Options.Create(jwtSettings));  
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();  
        
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
        
        return services;  
    }
}