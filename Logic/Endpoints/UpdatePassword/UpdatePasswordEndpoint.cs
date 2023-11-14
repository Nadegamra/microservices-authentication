using Authentication.Data.Repositories;
using Authentication.Models;
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
        private readonly IRepository<User> userRepository;
        private readonly CryptoService cryptoService;

        public UpdatePasswordEndpoint(CryptoService cryptoService, IRepository<User> userRepository)
        {
            this.cryptoService = cryptoService;
            this.userRepository = userRepository;
        }

        public override async Task HandleAsync(UpdatePasswordRequest req, CancellationToken ct)
        {
            var user = userRepository.Get(req.UserId);
            if (user is null || !user.PasswordHash.Equals(cryptoService.GetHash(req.OldPassword)))
            {
                AddError("Invalid current password");
                await SendErrorsAsync(400, ct);
                return;
            }
            user.PasswordHash = cryptoService.GetHash(req.NewPassword);

            userRepository.Update(user);

            await SendOkAsync(ct);
        }
    }
}
