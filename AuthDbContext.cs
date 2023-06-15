using Authentication.Models;
using Microsoft.EntityFrameworkCore;
namespace Authentication
{
    public class AuthDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<EmailConfirmationToken> EmailConfirmationTokens { get; set; }
        public DbSet<EmailChangeToken> EmailChangeTokens { get; set; }
        public DbSet<PasswordChangeToken> PasswordChangeTokens { get; set; }
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(new Role { Id = 1, Name = "admin", NormalizedName = "ADMIN" },
                                                new Role { Id = 2, Name = "creator", NormalizedName = "CREATOR" },
                                                new Role { Id = 3, Name = "consumer", NormalizedName = "CONSUMER" });

            builder.Entity<User>().HasData(new User { Id = 1, Username = "admin@example.com", NormalizedUsername = "ADMIN@EXAMPLE.COM", Email = "admin@example.com", EmailConfirmed = true, NormalizedEmail = "ADMIN@EXAMPLE.COM", PasswordHash = "0E63C20429D349EEED0C689DB2E47F1661927CFFFB8124DA456276854575367D0DED966F2C00D9D3A162B98DB8915371A880FB079C86E2AF9C04CC751A9BB9174FF0913A71ABD5CBE112EEAEC812709DB749234CF9ADDECE2937F2A1FC0BF2554E3EA3655EBFB712E0598B45C05FBF893084C7AA1ABF1F990603DD1D9B71400A" },
                                                new User { Id = 2, Username = "creator@example.com", NormalizedUsername = "CREATOR@EXAMPLE.COM", Email = "creator@example.com", EmailConfirmed = true, NormalizedEmail = "CREATOR@EXAMPLE.COM", PasswordHash = "0E63C20429D349EEED0C689DB2E47F1661927CFFFB8124DA456276854575367D0DED966F2C00D9D3A162B98DB8915371A880FB079C86E2AF9C04CC751A9BB9174FF0913A71ABD5CBE112EEAEC812709DB749234CF9ADDECE2937F2A1FC0BF2554E3EA3655EBFB712E0598B45C05FBF893084C7AA1ABF1F990603DD1D9B71400A" },
                                                new User { Id = 3, Username = "consumer@example.com", NormalizedUsername = "CONSUMER@EXAMPLE.COM", Email = "consumer@example.com", EmailConfirmed = true, NormalizedEmail = "CONSUMER@EXAMPLE.COM", PasswordHash = "0E63C20429D349EEED0C689DB2E47F1661927CFFFB8124DA456276854575367D0DED966F2C00D9D3A162B98DB8915371A880FB079C86E2AF9C04CC751A9BB9174FF0913A71ABD5CBE112EEAEC812709DB749234CF9ADDECE2937F2A1FC0BF2554E3EA3655EBFB712E0598B45C05FBF893084C7AA1ABF1F990603DD1D9B71400A" });

            builder.Entity<UserRole>().HasData(new UserRole { UserId = 1, RoleId = 1 },
                                                                 new UserRole { UserId = 2, RoleId = 2 },
                                                                 new UserRole { UserId = 3, RoleId = 3 });


            base.OnModelCreating(builder);
        }
    }

}
