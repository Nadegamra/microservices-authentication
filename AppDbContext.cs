using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Authentication
{
    public class AppDbContext : IdentityDbContext<IdentityUser<int>,IdentityRole<int>,int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole<int>>().HasData(new IdentityRole<int> { Id = 1, Name = "admin", NormalizedName = "ADMIN" },
                                                new IdentityRole<int> { Id = 2, Name = "creator", NormalizedName = "CREATOR" },
                                                new IdentityRole<int> { Id = 3, Name = "consumer", NormalizedName = "CONSUMER" });

            builder.Entity<IdentityUser<int>>().HasData(new IdentityUser<int> { Id = 1, UserName = "admin@admin.com", NormalizedUserName = "ADMIN@ADMIN.COM", Email = "admin@admin.com", EmailConfirmed = true, NormalizedEmail = "ADMIN@ADMIN.COM", PasswordHash = "AQAAAAEAACcQAAAAEK4hVsHx9G6FTUDDlJaY/l1aRXqpoUZU9nkEkvECUI2uQ+FHoFYHjlJpmP3KOss/qg==" },
                                                new IdentityUser<int> { Id = 2, UserName = "creator@example.com", NormalizedUserName = "CREATOR@EXAMPLE.COM", Email = "creator@example.com", EmailConfirmed = true, NormalizedEmail = "CREATOR@EXAMPLE.COM", PasswordHash = "AQAAAAEAACcQAAAAEK4hVsHx9G6FTUDDlJaY/l1aRXqpoUZU9nkEkvECUI2uQ+FHoFYHjlJpmP3KOss/qg==" },
            new IdentityUser<int> { Id = 3, UserName = "consumer@example.com", NormalizedUserName = "CONSUMER@EXAMPLE.COM", Email = "consumer@example.com", EmailConfirmed = true, NormalizedEmail = "CONSUMER@EXAMPLE.COM", PasswordHash = "AQAAAAEAACcQAAAAEK4hVsHx9G6FTUDDlJaY/l1aRXqpoUZU9nkEkvECUI2uQ+FHoFYHjlJpmP3KOss/qg==" });

            builder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int> { UserId = 1, RoleId = 1 },
                                                                 new IdentityUserRole<int> { UserId = 2, RoleId = 2 },
                                                                 new IdentityUserRole<int> { UserId = 3, RoleId = 3 });


            base.OnModelCreating(builder);
        }
    }

}
