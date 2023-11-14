using Authentication.Data.Repositories;
using Authentication.Models;
using FastEndpoints;
using FastEndpoints.Security;

namespace Authentication.Services
{
    public class TokenService : RefreshTokenService<TokenRequest, TokenResponse>
    {
        private readonly IRepository<UserToken> userTokenRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<UserRole> userRoleRepository;
        private readonly IRepository<Role> roleRepository;

        private readonly CryptoService cryptoService;
        public TokenService(IConfiguration config, CryptoService cryptoService, IRepository<UserToken> userTokenRepository, IRepository<User> userRepository, IRepository<UserRole> userRoleRepository, IRepository<Role> roleRepository)
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
            this.cryptoService = cryptoService;
            this.userTokenRepository = userTokenRepository;
            this.userRepository = userRepository;
            this.userRoleRepository = userRoleRepository;
            this.roleRepository = roleRepository;
        }

        public override async Task PersistTokenAsync(TokenResponse response)
        {
            userTokenRepository.Add(new UserToken
            {
                UserId = Convert.ToInt32(response.UserId),
                RefreshTokenHash = cryptoService.GetHash(response.RefreshToken),
                AccessExpiry = response.AccessExpiry,
                RefreshExpiry = response.RefreshExpiry,
                Used = false,
            });
        }

        public override async Task RefreshRequestValidationAsync(TokenRequest req)
        {
            var token = userTokenRepository.GetAll().Where(x => x.UserId.ToString() == req.UserId && cryptoService.GetHash(req.RefreshToken) == x.RefreshTokenHash && x.RefreshExpiry > DateTime.Now).FirstOrDefault();

            if (token == null)
            {
                AddError(r => r.RefreshToken, "Refresh token is invalid!");
                return;
            }

            if (token.Used)
            {
                var userTokens = userTokenRepository.GetAll().Where(x => x.UserId.ToString() == req.UserId).ToList();
                userTokens.ForEach(x => userTokenRepository.Delete(x));
                AddError(r => r.RefreshToken, "Refresh token is invalid!");
                return;
            }

            token.Used = true;
            userTokenRepository.Update(token);

        }

        public override async Task SetRenewalPrivilegesAsync(TokenRequest request, UserPrivileges privileges)
        {
            var user = userRepository.Get(int.Parse(request.UserId));
            if (user == null)
            {
                throw new Exception("User with this id does not exist");
            }

            var userRole = userRoleRepository.GetAll().Where(x => x.UserId == user.Id).First().RoleId;
            string roleName = roleRepository.Get(userRole)!.NormalizedName;

            privileges.Roles.Add(roleName);

            privileges.Claims.Add(new("UserId", request.UserId));
            privileges.Claims.Add(new("UserEmail", user.Email));
            privileges.Claims.Add(new("UserName", user.Username));
        }
    }
}
