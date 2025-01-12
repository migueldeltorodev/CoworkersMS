using ManagementSystem.Api.Common.Extensions;
using ManagementSystem.Api.Endpoints.Booking;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
{
    // Services
    builder.Services.AddOpenApi();
    builder.Services.AddDatabase(builder.Configuration);
    builder.Services.AddAuth(builder.Configuration);
    builder.Services.AddRepositories();
    builder.Services.AddMediatR();
    builder.Services.AddValidators();
    
    // Mapper profiles
    builder.Services.AddAutoMapper(typeof(Program));
    
    // Add Swagger services
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "ManagementSystem API", Version = "v1" });
    });
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();
        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ManagementSystem API V1");
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    
    app.Run();
}

