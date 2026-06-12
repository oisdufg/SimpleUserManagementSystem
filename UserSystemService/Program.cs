using Microsoft.AspNetCore.Builder;
using UserSystemService.Context;
using UserSystemService.Repositories;
using UserSystemService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserSystemService.Interfaces;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
string connectionString = configuration
    .GetConnectionString("DefaultConnection") 
                          ?? throw new InvalidOperationException("Connection string not found");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddGrpc();

WebApplication app = builder.Build();

app.MapGrpcService<UserService>();

app.Run();