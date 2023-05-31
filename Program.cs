using Authentication;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

{
    services.AddCors();
    services.AddAuthentication().AddJwtBearer();
    services.AddAuthorization();
    services.AddIdentity<IdentityUser<int>, IdentityRole<int>>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddTokenProvider<DataProtectorTokenProvider<IdentityUser<int>>>(TokenOptions.DefaultProvider);

    services.AddFastEndpoints();

    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    // Change to following when doing `dotnet ef database update`
    // string connectionString = builder.Configuration.GetConnectionString("MigrationConnection");
    services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddTransient<UserManager<IdentityUser<int>>>();
    services.AddTransient<RoleManager<IdentityRole<int>>>();
}


var app = builder.Build();

{
    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();
    app.UseFastEndpoints();

}

app.Run();
