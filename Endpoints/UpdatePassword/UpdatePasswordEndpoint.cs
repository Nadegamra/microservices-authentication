using Authentication.Services;
using FastEndpoints;

namespace Authentication.Endpoints.UpdatePassword
{
    public class UpdatePasswordEndpoint : Endpoint<UpdatePasswordRequest, EmptyResponse>
    {
        public override void Configure()
        {
            Put("auth/updatePassword");
        }

        private readonly AuthDbContext appDbContext;
        private readonly CryptoService cryptoService;

        public UpdatePasswordEndpoint(AuthDbContext appDbContext, CryptoService cryptoService)
        {
            this.appDbContext = appDbContext;
            this.cryptoService = cryptoService;
        }

        public override async Task HandleAsync(UpdatePasswordRequest req, CancellationToken ct)
        {
            var user = appDbContext.Users.Where(x => x.Id == req.UserId).FirstOrDefault();
            if (user is null || !user.PasswordHash.Equals(cryptoService.GetHash(req.OldPassword)))
            {
                await SendErrorsAsync(400, ct);
                return;
            }
            user.PasswordHash = cryptoService.GetHash(req.NewPassword);
            appDbContext.Users.Update(user);
            appDbContext.SaveChanges();
            await SendOkAsync(ct);
        }
    }
}
