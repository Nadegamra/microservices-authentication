using System.Text.Json.Serialization;
using Authentication;
using Authentication.BackgroundTasks;
using Authentication.Data.Repositories;
using Authentication.Enums;
using Authentication.Models;
using Authentication.Properties;
using Authentication.Services;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Services.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
{
    services.AddCors((options) =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("http://localhost", "https://localhost", "http://localhost:3000", "https://nadegamraolpfrontend.azurewebsites.net")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
    });

    services.AddHostedService<ClearExpiredRefreshTokens>();
    services.AddHostedService<UserDeletion>();

    services.AddFastEndpoints();
    services.AddJWTBearerAuth(builder.Configuration["JWT:Secret"]);
    services.SwaggerDocument();

    // Configuration
    services.Configure<SmtpConfig>(builder.Configuration.GetSection("Smtp"));
    services.Configure<IPConfig>(builder.Configuration.GetSection("IP"));

    services.AddScoped<IRepository<EmailChangeToken>, EmailChangeTokenRepository>();
    services.AddScoped<IRepository<EmailConfirmationToken>, EmailConfirmationTokenRepository>();
    services.AddScoped<IRepository<PasswordChangeToken>, PasswordChangeTokenRepository>();
    services.AddScoped<IRepository<Authentication.Models.Role>, RoleRepository>();
    services.AddScoped<IRepository<User>, UserRepository>();
    services.AddScoped<IRepository<UserRole>, UserRoleRepository>();
    services.AddScoped<IRepository<UserToken>, UserTokenRepository>();

    // Services
    services.AddTransient<TokenService>();
    services.AddTransient<CryptoService>();
    services.AddTransient<EmailService>();

    string connectionString = builder.Configuration.GetSection("Database")["ConnectionString"];// Change to "MigrationConnection" when updating the database
    services.AddDbContext<AuthDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

    // Event Bus
    ConfigureServices.AddEventBus(builder);
}
var app = builder.Build();
{
    // app.UseHttpsRedirection();

    app.UseCors();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseFastEndpoints(c =>
    {
        c.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
    });

    app.UseSwaggerGen();

    var eventBus = app.Services.GetRequiredService<Infrastructure.EventBus.Generic.IEventBus>();
}
app.Run();
