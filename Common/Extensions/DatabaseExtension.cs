using ManagementSystem.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Api.Common.Extensions;

public static class DatabaseExtension
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        3,
                        TimeSpan.FromSeconds(30),
                        null);
                    sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name);
                });
        });

        return services;
    }
}