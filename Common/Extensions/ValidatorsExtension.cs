using System.Reflection;
using FluentValidation;

namespace ManagementSystem.Api.Common.Extensions;

public static class ValidatorsExtension
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}