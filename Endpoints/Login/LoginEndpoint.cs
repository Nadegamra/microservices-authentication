using Authentication.Endpoints.Login;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Authentication.Endpoints.Register
{
    public class LoginEndpoint: Endpoint<LoginRequest, LoginResponse,LoginMapper>
    {
        public override void Configure()
        {
            Post("login");
            AllowAnonymous();
        }

        private readonly UserManager<IdentityUser<int>> userManager;
        private readonly SignInManager<IdentityUser<int>> signInManager;

        public LoginEndpoint(UserManager<IdentityUser<int>> userManager, SignInManager<IdentityUser<int>> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
        {
            var user = await userManager.FindByEmailAsync(req.Email);
            if (user is null)
            {
                throw new ArgumentException("Login credentials are incorrect");
            }
            if (!user.EmailConfirmed)
            {
                throw new ArgumentException("Email has not been confirmed yet");
            }
            var signInResult = await signInManager.PasswordSignInAsync(user, req.Password, req.RememberPassword, false);

            if (!signInResult.Succeeded)
            {
                throw new ArgumentException("Login credentials are incorrect");
            }
            Response = Map.FromEntity(user);
            Response.Role = (await userManager.GetRolesAsync(user)).First();

            await SendOkAsync(Response);
        }
    }
}
