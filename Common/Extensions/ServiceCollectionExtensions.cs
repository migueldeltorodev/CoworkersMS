using System.Reflection;
using FluentValidation;
using ManagementSystem.Api.Common.Behaviors;
using ManagementSystem.Api.Common.Interfaces;
using ManagementSystem.Api.Persistence.Repositories;
using MediatR;

namespace ManagementSystem.Api.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();

        return services;
    }
}