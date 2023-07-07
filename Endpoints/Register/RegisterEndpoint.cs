using Authentication.Models;
using Authentication.Properties;
using Authentication.Services;
using FastEndpoints;
using Microsoft.Extensions.Options;

namespace Authentication.Endpoints.Register
{
    public class RegisterEndpoint : Endpoint<RegisterRequest, EmptyResponse, RegisterMapper>
    {
        public override void Configure()
        {
            Post("auth/register");
            AllowAnonymous();
        }

        private readonly AuthDbContext appDbContext;
        private readonly EmailService emailService;
        private readonly CryptoService cryptoService;
        private readonly IOptions<IPConfig> ipConfig;

        public RegisterEndpoint(AuthDbContext appDbContext, EmailService emailService, CryptoService cryptoService, IOptions<IPConfig> ipConfig)
        {
            this.appDbContext = appDbContext;
            this.emailService = emailService;
            this.cryptoService = cryptoService;
            this.ipConfig = ipConfig;
        }

        public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
        {
            User user = Map.ToEntity(req);
            user.PasswordHash = cryptoService.GetHash(req.Password);

            if (appDbContext.Users.Where(x => x.NormalizedEmail.Equals(req.Email.ToUpper())).FirstOrDefault() != null)
            {
                AddError("Email already exists");
                await SendErrorsAsync(400, ct);
                return;
            }

            var result = appDbContext.Users.Add(user);
            appDbContext.SaveChanges();

            UserRole userRole = new UserRole
            {
                UserId = result.Entity.Id,
                RoleId = (Enums.Role)req.Role == Enums.Role.Consumer ? 3 : 2,//TODO: increase clarity
            };
            appDbContext.UserRoles.Add(userRole);

            EmailConfirmationToken token = new()
            {
                UserId = result.Entity.Id,
                Token = cryptoService.GenerateRandomUrlSafeBase64String(64)
            };
            appDbContext.EmailConfirmationTokens.Add(token);

            appDbContext.SaveChanges();

            string emailSubject = "Account confirmation";
            string emailBody = $"<div>If you have not created this account, you can ignore this email.<br/>Your email confirmation link:<br/>http://{ipConfig.Value.Address}:{ipConfig.Value.Port}/confirmEmail/{token.Token}</div>";
            emailService.SendEmail(req.Email, emailSubject, emailBody);

            await SendOkAsync(ct);
        }
    }
}
