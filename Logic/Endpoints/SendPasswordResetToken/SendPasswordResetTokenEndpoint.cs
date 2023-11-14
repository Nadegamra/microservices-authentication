using Authentication.Data.Repositories;
using Authentication.Models;
using Authentication.Properties;
using Authentication.Services;
using FastEndpoints;
using Microsoft.Extensions.Options;

namespace Authentication.Endpoints.SendPasswordResetToken
{
    public class SendPasswordResetTokenEndpoint : Endpoint<SendPasswordResetTokenRequest>
    {
        public override void Configure()
        {
            Post("auth/sendPasswordResetToken");
            AllowAnonymous();
        }

        private readonly IRepository<User> userRepository;
        private readonly IRepository<PasswordChangeToken> tokenRepository;
        private readonly CryptoService cryptoService;
        private readonly EmailService emailService;
        private readonly IOptions<IPConfig> ipConfig;

        public SendPasswordResetTokenEndpoint(CryptoService cryptoService, EmailService emailService, IOptions<IPConfig> ipConfig, IRepository<User> userRepository, IRepository<PasswordChangeToken> tokenRepository)
        {
            this.cryptoService = cryptoService;
            this.emailService = emailService;
            this.ipConfig = ipConfig;
            this.userRepository = userRepository;
            this.tokenRepository = tokenRepository;
        }

        public override async Task HandleAsync(SendPasswordResetTokenRequest req, CancellationToken ct)
        {
            User? user = userRepository.GetAll().Where(x => x.NormalizedEmail == req.EmailAddress.ToUpper()).FirstOrDefault();
            if (user == null)
            {
                await SendErrorsAsync(400, ct);
                return;
            }
            PasswordChangeToken token = new()
            {
                UserId = user.Id,
                Token = cryptoService.GenerateRandomUrlSafeBase64String(64),
            };

            tokenRepository.Add(token);

            string emailSubject = "Password reset";
            string emailBody = $"<div>If you have not requested a password reset, you can ignore this email.<br/>Your password reset link:<br/>https://{ipConfig.Value.Address}/changePassword/{token.Token}</div>";
            emailService.SendEmail(user.Email, emailSubject, emailBody);

            await SendOkAsync(ct);
        }
    }
}
