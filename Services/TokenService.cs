using Authentication.Endpoints.Login;
using Authentication.Models;
using FastEndpoints;
using FastEndpoints.Security;

namespace Authentication.Services
{
    public class TokenService : RefreshTokenService<LoginRequest, LoginResponse>
    {
        private readonly AppDbContext appDbContext;
        public TokenService(IConfiguration config, AppDbContext appDbContext)
        {
            Setup(o =>
            {
                o.TokenSigningKey = "Key+F0rTOk&n+Sig=1n6";
                o.AccessTokenValidity = TimeSpan.FromMinutes(5);
                o.RefreshTokenValidity = TimeSpan.FromHours(4);

                o.Endpoint("/auth/refresh-token", ep =>
                {
                    ep.Summary(s => s.Summary = "this is the refresh token endpoint");
                });
            });
            this.appDbContext = appDbContext;
        }

        public override async Task PersistTokenAsync(LoginResponse response)
        {
            appDbContext.UserTokens.Add(new UserToken
            {
                UserId = Convert.ToInt32(response.UserId),
                AccessToken = response.RefreshToken,
                AccessExpiry = response.AccessExpiry,
                RefreshExpiry = response.RefreshExpiry,
            });
            appDbContext.SaveChanges();
        }

        public override async Task RefreshRequestValidationAsync(LoginRequest req)
        {
            var token = appDbContext.UserTokens.Where(x => x.UserId.ToString().Equals(req.UserId) && req.RefreshToken == x.AccessToken && x.RefreshExpiry > DateTime.Now).FirstOrDefault();

            if (token == null)
            {
                AddError(r => r.RefreshToken, "Refresh token is invalid!");
            }
        }

        public override async Task SetRenewalPrivilegesAsync(LoginRequest request, UserPrivileges privileges)
        {
            privileges.Roles.AddRange(privileges.Roles);
            privileges.Claims.AddRange(privileges.Claims);
            privileges.Permissions.AddRange(privileges.Permissions);
        }
    }
}
