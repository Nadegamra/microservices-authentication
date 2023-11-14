using Authentication.Data.Repositories;
using Authentication.Models;
using Authentication.Properties;
using Authentication.Services;
using FastEndpoints;
using Microsoft.Extensions.Options;

namespace Authentication.Endpoints.SendEmailChangeToken
{
    public class SendEmailChangeTokenEndpoint : Endpoint<SendEmailChangeTokenRequest, EmptyResponse, SendEmailChangeTokenMapper>
    {
        public override void Configure()
        {
            Post("auth/sendEmailChangeToken");
        }

        private readonly IRepository<User> userRepository;
        private readonly IRepository<EmailChangeToken> tokenRepository;
        private readonly CryptoService cryptoService;
        private readonly EmailService emailService;
        private readonly IOptions<IPConfig> ipConfig;

        public SendEmailChangeTokenEndpoint(CryptoService cryptoService, EmailService emailService, IOptions<IPConfig> ipConfig, IRepository<User> userRepository, IRepository<EmailChangeToken> tokenRepository)
        {
            this.cryptoService = cryptoService;
            this.emailService = emailService;
            this.ipConfig = ipConfig;
            this.userRepository = userRepository;
            this.tokenRepository = tokenRepository;
        }

        public override async Task HandleAsync(SendEmailChangeTokenRequest req, CancellationToken ct)
        {
            User? user = userRepository.Get(req.UserId);
            if (user == null)
            {
                await SendErrorsAsync(400, ct);
                return;
            }
            EmailChangeToken token = Map.ToEntity(req);
            token.Token = cryptoService.GenerateRandomUrlSafeBase64String(64);

            tokenRepository.Add(token);

            string emailSubject = "Email Change";
            string emailBody = $"<div>You have requested to change your email to {req.EmailAddress}. If you have not initiated this action, your account may have been compromised.<br/>Your email change link:<br/>https://{ipConfig.Value.Address}/changeEmail/{token.Token}</div>";
            emailService.SendEmail(user.Email, emailSubject, emailBody);

            await SendOkAsync(ct);
        }
    }
}
