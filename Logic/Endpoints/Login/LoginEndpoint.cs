using FastEndpoints;
using Authentication.Services;
using FastEndpoints.Security;
using Authentication.Data.Repositories;
using Authentication.Models;

namespace Authentication.Endpoints.Login
{
    public class LoginEndpoint : Endpoint<LoginRequest, TokenResponse>
    {
        public override void Configure()
        {
            Post("auth/login");
            AllowAnonymous();
        }

        private readonly IRepository<User> userRepository;
        private readonly IRepository<UserRole> userRoleRepository;
        private readonly IRepository<Role> roleRepository;
        private readonly CryptoService hashingService;

        public LoginEndpoint(CryptoService hashingService, IRepository<User> userRepository, IRepository<UserRole> userRoleRepository, IRepository<Role> roleRepository)
        {
            this.hashingService = hashingService;
            this.userRepository = userRepository;
            this.userRoleRepository = userRoleRepository;
            this.roleRepository = roleRepository;
        }

        public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
        {
            var user = userRepository.GetAll().Where(x => x.Email == req.Email).FirstOrDefault();
            if (user is null || hashingService.GetHash(req.Password) != user.PasswordHash || !user.EmailConfirmed)
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            var userRole = userRoleRepository.GetAll().Where(x => x.UserId == user.Id).First().RoleId;
            string roleName = roleRepository.Get(userRole)!.NormalizedName;

            user.IsDeleted = false;
            user.DeletedAt = null;

            Response = await CreateTokenWith<TokenService>(user.Id.ToString(), u =>
            {
                u.Roles.Add(roleName);
                u.Claims.Add(new("UserId", user.Id.ToString()));
                u.Claims.Add(new("UserEmail", user.Email));
                u.Claims.Add(new("UserName", user.Username));
            });
            await SendOkAsync(Response, ct);
        }

    }
}
