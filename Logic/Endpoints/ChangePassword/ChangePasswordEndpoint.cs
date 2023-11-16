using Authentication.Data.Repositories;
using Authentication.Models;
using Authentication.Services;
using FastEndpoints;

namespace Authentication.Endpoints.ChangePassword
{
    public class ChangePasswordEndpoint : Endpoint<ChangePasswordRequest, EmptyResponse>
    {
        public override void Configure()
        {
            Put("auth/changePassword");
            AllowAnonymous();
        }

        private readonly IRepository<User> userRepository;
        private readonly IRepository<PasswordChangeToken> tokenRepository;
        private readonly CryptoService cryptoService;

        public ChangePasswordEndpoint(CryptoService cryptoService, IRepository<User> userRepository, IRepository<PasswordChangeToken> tokenRepository)
        {
            this.cryptoService = cryptoService;
            this.userRepository = userRepository;
            this.tokenRepository = tokenRepository;
        }

        public override async Task HandleAsync(ChangePasswordRequest req, CancellationToken ct)
        {
            PasswordChangeToken? token = tokenRepository.GetAll().Where(x => x.Token == req.Token).FirstOrDefault();
            if (token == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var user = userRepository.Get(token.UserId);
            if (user == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            user.PasswordHash = cryptoService.GetHash(req.NewPassword);

            userRepository.Update(user);

            tokenRepository.Delete(token);

            await SendNoContentAsync(ct);
        }
    }
}
