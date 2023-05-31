using FastEndpoints;
using System.Security.Cryptography;
using System.Text;
using Authentication.Services;

namespace Authentication.Endpoints.GetToken
{
    public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
    {
        public override void Configure()
        {
            Post("auth/login");
            AllowAnonymous();
        }

        private readonly AppDbContext appDbContext;

        public LoginEndpoint(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
        {
            if (!CheckUser(req.Email, req.Password))
            {
                await SendErrorsAsync(400, ct);
                return;
            }

            var user = appDbContext.Users.Where(x => x.Email == req.Email).First();
            var userRole = appDbContext.UserRoles.Where(x => x.UserId == user.Id).First().RoleId;
            string roleName = appDbContext.Roles.Where(x => x.Id == userRole).First().NormalizedName;

            Response = await CreateTokenWith<TokenService>(user.Id.ToString(), u =>
            {
                u.Roles.Add(roleName);
                //u.Permissions.AddRange(new[] { "ManageUsers", "ManageInventory" });
                u.Claims.Add(new("UserId", user.Id.ToString()));
                u.Claims.Add(new("UserEmail", user.Email));
            });

            //var jwtToken = JWTBearer.CreateToken(
            //    signingKey: "Key+F0rTOk&n+Sig=1n6",
            //    expireAt: DateTime.UtcNow.AddDays(1),
            //    priviledges: u =>
            //    {
            //        u.Roles.Add(roleName);
            //        //u.Permissions.AddRange(new[] { "ManageUsers", "ManageInventory" });
            //        u.Claims.Add(new("UserId", user.Id.ToString()));
            //        u.Claims.Add(new("UserEmail", user.Email));
            //    });

            //Response.AccessToken = jwtToken;
            await SendOkAsync(Response, ct);
        }

        private bool CheckUser(string email, string password)
        {
            var user = appDbContext.Users.Where(x => x.Email.ToUpper() == email.ToUpper()).FirstOrDefault();
            if (user == null)
            {
                return false;
            }

            return HashPassword(password) == user.PasswordHash;
        }

        private string HashPassword(string password)
        {
            const string salt = "Ac15!.=";
            return Convert.ToHexString(Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                Encoding.UTF8.GetBytes(salt),
                350000,
                HashAlgorithmName.SHA512,
                128));
        }
    }
}
