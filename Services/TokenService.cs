using Authentication.Endpoints.Login;
using Authentication.Models;
using FastEndpoints;
using FastEndpoints.Security;

namespace Authentication.Services
{
    public class TokenService : RefreshTokenService<TokenRequest, TokenResponse>
    {
        private readonly AuthDbContext appDbContext;
        private readonly CryptoService cryptoService;
        public TokenService(IConfiguration config, AuthDbContext appDbContext, CryptoService cryptoService)
        {
            Setup(o =>
            {
                o.TokenSigningKey = config["JWT:Secret"];
                o.AccessTokenValidity = TimeSpan.FromMinutes(5);
                o.RefreshTokenValidity = TimeSpan.FromHours(4);

                o.Endpoint("/auth/refresh-token", ep =>
                {
                    ep.Summary(s => s.Summary = "this is the refresh token endpoint");
                });
            });
            this.appDbContext = appDbContext;
            this.cryptoService = cryptoService;
        }

        public override async Task PersistTokenAsync(TokenResponse response)
        {
            appDbContext.UserTokens.Add(new UserToken
            {
                UserId = Convert.ToInt32(response.UserId),
                RefreshTokenHash = cryptoService.GetHash(response.RefreshToken),
                AccessExpiry = response.AccessExpiry,
                RefreshExpiry = response.RefreshExpiry,
                Used = false,
            });
            appDbContext.SaveChanges();
        }

        public override async Task RefreshRequestValidationAsync(TokenRequest req)
        {
            var token = appDbContext.UserTokens.Where(x => x.UserId.ToString() == req.UserId && cryptoService.GetHash(req.RefreshToken) == x.RefreshTokenHash && x.RefreshExpiry > DateTime.Now).FirstOrDefault();

            if (token == null)
            {
                AddError(r => r.RefreshToken, "Refresh token is invalid!");
                return;
            }

            if (token.Used)
            {
                var userTokens = appDbContext.UserTokens.Where(x => x.UserId.ToString() == req.UserId).ToList();
                appDbContext.UserTokens.RemoveRange(userTokens);
                appDbContext.SaveChanges();
                AddError(r => r.RefreshToken, "Refresh token is invalid!");
                return;
            }

            token.Used = true;
            appDbContext.UserTokens.Update(token);
            appDbContext.SaveChanges();

        }

        public override async Task SetRenewalPrivilegesAsync(TokenRequest request, UserPrivileges privileges)
        {
            var user = appDbContext.Users.Where(x => x.Id.ToString() == request.UserId).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("User with this id does not exist");
            }

            var userRole = appDbContext.UserRoles.Where(x => x.UserId == user.Id).First().RoleId;
            string roleName = appDbContext.Roles.Where(x => x.Id == userRole).First().NormalizedName;

            privileges.Roles.Add(roleName);

            privileges.Claims.Add(new("UserId", request.UserId));
            privileges.Claims.Add(new("UserEmail", user.Email));
            privileges.Claims.Add(new("UserName", user.Username));
        }
    }
}
