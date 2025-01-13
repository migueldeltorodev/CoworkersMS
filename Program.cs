using ManagementSystem.Api.Common.Extensions;
using ManagementSystem.Api.Endpoints.Booking;
using ManagementSystem.Api.Endpoints.Room;
using ManagementSystem.Api.Endpoints.User;
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
        c.SwaggerDoc("v1", new OpenApiInfo { 
            Title = "ManagementSystem API", 
            Version = "v1" 
        });
    });
    
    // Cors
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(c =>
        {
            c.SerializeAsV2 = false;
        });
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ManagementSystem API V1");
        });
    }
    
    app.UseCors("AllowAll");
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapBookingEndpoints();
    app.MapUserEndpoints();
    app.MapRoomEndpoints();
    
    app.Run();
}

