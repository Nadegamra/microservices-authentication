using FastEndpoints;
using Authentication.Services;

namespace Authentication.Endpoints.Login
{
    public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
    {
        public override void Configure()
        {
            Post("auth/login");
            AllowAnonymous();
        }

        private readonly AuthDbContext appDbContext;
        private readonly CryptoService hashingService;

        public LoginEndpoint(AuthDbContext appDbContext, CryptoService hashingService)
        {
            this.appDbContext = appDbContext;
            this.hashingService = hashingService;
        }

        public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
        {
            var user = appDbContext.Users.Where(x => x.Email == req.Email).FirstOrDefault();
            if (user is null || hashingService.GetHash(req.Password) != user.PasswordHash || !user.EmailConfirmed)
            {
                AddError("Invalid credentials");
                await SendErrorsAsync(400, ct);
                return;
            }

            var userRole = appDbContext.UserRoles.Where(x => x.UserId == user.Id).First().RoleId;
            string roleName = appDbContext.Roles.Where(x => x.Id == userRole).First().NormalizedName;

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
