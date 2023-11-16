using Authentication.Data.Repositories;
using Authentication.IntegrationEvents.Events;
using Authentication.Models;
using FastEndpoints;

namespace Authentication.Endpoints.ConfirmEmail
{
    public class ConfirmEmailEndpoint : Endpoint<ConfirmEmailRequest>
    {
        public override void Configure()
        {
            Put("auth/confirmEmail");
            AllowAnonymous();
        }

        private readonly IRepository<EmailConfirmationToken> tokenRepository;
        private readonly IRepository<User> userRepository;
        private readonly Infrastructure.EventBus.Generic.IEventBus eventBus;

        public ConfirmEmailEndpoint(Infrastructure.EventBus.Generic.IEventBus eventBus, IRepository<EmailConfirmationToken> tokenRepository, IRepository<User> userRepository)
        {
            this.eventBus = eventBus;
            this.tokenRepository = tokenRepository;
            this.userRepository = userRepository;
        }

        public override async Task HandleAsync(ConfirmEmailRequest req, CancellationToken ct)
        {
            var token = tokenRepository.GetAll().Where(x => x.Token.Equals(req.Token)).FirstOrDefault();
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

            user.EmailConfirmed = true;

            userRepository.Update(user);
            tokenRepository.Delete(token);

            eventBus.Publish(new CreatorRegisteredIntegrationEvent()
            {
                UserId = user.Id,
                Email = user.Email,
                Username = user.Username,
            });

            await SendNotFoundAsync(ct);
            return;
        }
    }
}
