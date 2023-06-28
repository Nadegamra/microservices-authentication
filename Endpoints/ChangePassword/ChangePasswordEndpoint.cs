using Authentication.Models;
using Authentication.Services;
using FastEndpoints;

namespace Authentication.Endpoints.ChangePassword
{
    public class ChangePasswordEndpoint : Endpoint<ChangePasswordRequest, EmptyResponse>
    {
        public override void Configure()
        {
            Post("auth/changePassword");
            AllowAnonymous();
        }

        private readonly AuthDbContext appDbContext;
        private readonly CryptoService cryptoService;

        public ChangePasswordEndpoint(AuthDbContext appDbContext, CryptoService cryptoService)
        {
            this.appDbContext = appDbContext;
            this.cryptoService = cryptoService;
        }

        public override async Task HandleAsync(ChangePasswordRequest req, CancellationToken ct)
        {
            PasswordChangeToken? token = appDbContext.PasswordChangeTokens.Where(x => x.Token == req.Token).FirstOrDefault();
            if (token == null)
            {
                AddError("Invalid token");
                await SendErrorsAsync(400, ct);
                return;
            }

            var user = appDbContext.Users.Where(x => x.Id == token.UserId).FirstOrDefault();
            if (user == null)
            {
                AddError("Invalid token");
                await SendErrorsAsync(400, ct);
                return;
            }

            user.PasswordHash = cryptoService.GetHash(req.NewPassword);
            appDbContext.Users.Update(user);

            appDbContext.PasswordChangeTokens.Remove(token);

            appDbContext.SaveChanges();

            await SendOkAsync(ct);
        }
    }
}
