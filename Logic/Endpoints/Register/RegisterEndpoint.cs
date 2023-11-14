using Authentication.Data.Repositories;
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

        private readonly IRepository<User> userRepository;
        private readonly IRepository<UserRole> userRoleRepository;
        private readonly IRepository<EmailConfirmationToken> tokenRepository;
        private readonly EmailService emailService;
        private readonly CryptoService cryptoService;
        private readonly IOptions<IPConfig> ipConfig;

        public RegisterEndpoint(EmailService emailService, CryptoService cryptoService, IOptions<IPConfig> ipConfig, IRepository<User> userRepository, IRepository<UserRole> userRoleRepository, IRepository<EmailConfirmationToken> tokenRepository)
        {
            this.emailService = emailService;
            this.cryptoService = cryptoService;
            this.ipConfig = ipConfig;
            this.userRepository = userRepository;
            this.userRoleRepository = userRoleRepository;
            this.tokenRepository = tokenRepository;
        }

        public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
        {
            User user = Map.ToEntity(req);
            user.PasswordHash = cryptoService.GetHash(req.Password);

            if (userRepository.GetAll().Where(x => x.NormalizedEmail.Equals(req.Email.ToUpper())).FirstOrDefault() != null)
            {
                AddError("Email already exists");
                await SendErrorsAsync(400, ct);
                return;
            }

            var result = userRepository.Add(user);

            UserRole userRole = new UserRole
            {
                UserId = result.Id,
                RoleId = (int)req.Role,
            };

            userRoleRepository.Add(userRole);

            EmailConfirmationToken token = new()
            {
                UserId = result.Id,
                Token = cryptoService.GenerateRandomUrlSafeBase64String(64)
            };

            tokenRepository.Add(token);

            string emailSubject = "Account confirmation";
            string emailBody = $"<div>If you have not created this account, you can ignore this email.<br/>Your email confirmation link:<br/>https://{ipConfig.Value.Address}/confirmEmail/{token.Token}</div>";
            emailService.SendEmail(req.Email, emailSubject, emailBody);

            await SendOkAsync(ct);
        }
    }
}
