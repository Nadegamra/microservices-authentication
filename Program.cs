using Authentication;
using Authentication.BackgroundTasks;
using Authentication.Properties;
using Authentication.Services;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
{
    services.AddCors((options) =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("https://localhost:3000", "http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
    });

    services.AddHostedService<ClearExpiredRefreshTokens>();

    services.AddFastEndpoints();
    services.AddJWTBearerAuth(builder.Configuration["JwtSecret"]);
    services.AddSwaggerDoc();

    services.Configure<SmtpConfig>(builder.Configuration.GetSection("Smtp"));
    services.Configure<IPConfig>(builder.Configuration.GetSection("IP"));

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
    app.UseHttpsRedirection();

    app.UseCors();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseFastEndpoints();

    app.UseSwaggerGen();

    var eventBus = app.Services.GetRequiredService<Infrastructure.EventBus.Generic.IEventBus>();
}
app.Run();
