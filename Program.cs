using ManagementSystem.Api.Common.Extensions;

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
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    
    app.Run();
}

