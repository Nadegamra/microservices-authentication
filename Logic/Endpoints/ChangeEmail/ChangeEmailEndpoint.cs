using Authentication.Data.Repositories;
using Authentication.IntegrationEvents.Events;
using Authentication.Models;
using FastEndpoints;

namespace Authentication.Endpoints.ChangeEmail
{
    public class ChangeEmailEndpoint : Endpoint<ChangeEmailRequest, EmptyResponse>
    {
        public override void Configure()
        {
            Post("auth/changeEmail");
            AllowAnonymous();
        }
        private readonly IRepository<User> userRepository;
        private readonly IRepository<EmailChangeToken> tokenRepository;
        private readonly Infrastructure.EventBus.Generic.IEventBus eventBus;

        public ChangeEmailEndpoint(Infrastructure.EventBus.Generic.IEventBus eventBus, IRepository<User> userRepository, IRepository<EmailChangeToken> tokenRepository)
        {
            this.eventBus = eventBus;
            this.userRepository = userRepository;
            this.tokenRepository = tokenRepository;
        }

        public override async Task HandleAsync(ChangeEmailRequest req, CancellationToken ct)
        {
            EmailChangeToken? token = tokenRepository.GetAll().Where(x => x.Token == req.Token).FirstOrDefault();
            if (token == null)
            {
                AddError("Invalid token");
                await SendErrorsAsync(400, ct);
                return;
            }

            var user = userRepository.Get(token.UserId);
            if (user == null)
            {
                AddError("Invalid token");
                await SendErrorsAsync(400, ct);
                return;
            }

            string oldEmail = user.Email;
            string oldUsername = user.Username;

            user.Email = token.EmailAddress;
            user.NormalizedEmail = token.EmailAddress.ToUpper();

            userRepository.Update(user);

            eventBus.Publish(new UserEmailChangedIntegrationEvent()
            {
                NewEmail = token.EmailAddress,
                OldEmail = oldEmail,
                UserId = user.Id
            });

            tokenRepository.Delete(token);

            await SendOkAsync(ct);

        }
    }
}
