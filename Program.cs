using Authentication;
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
    services.AddJWTBearerAuth("Key+F0rTOk&n+Sig=1n6");
    services.AddSwaggerDoc();

    services.AddTransient<TokenService>();

    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");// Change to "MigrationConnection" when updating the database
    services.AddDbContext<AppDbContext>(options =>
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
