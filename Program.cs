using Authentication;
using Authentication.Properties;
using Authentication.Services;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
{
    services.AddCors();

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
}
app.Run();
