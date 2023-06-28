using Authentication.IntegrationEvents.Events;
using FastEndpoints;

namespace Authentication.Endpoints.ConfirmEmail
{
    public class ConfirmEmailEndpoint : Endpoint<ConfirmEmailRequest>
    {
        public override void Configure()
        {
            Post("auth/confirmEmail");
            AllowAnonymous();
        }

        private readonly AuthDbContext appDbContext;
        private readonly Infrastructure.EventBus.Generic.IEventBus eventBus;

        public ConfirmEmailEndpoint(AuthDbContext appDbContext, Infrastructure.EventBus.Generic.IEventBus eventBus)
        {
            this.appDbContext = appDbContext;
            this.eventBus = eventBus;
        }

        public override async Task HandleAsync(ConfirmEmailRequest req, CancellationToken ct)
        {
            var token = appDbContext.EmailConfirmationTokens.Where(x => x.Token.Equals(req.Token)).FirstOrDefault();
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

            user.EmailConfirmed = true;
            appDbContext.Users.Update(user);
            appDbContext.EmailConfirmationTokens.Remove(token);
            appDbContext.SaveChanges();

            eventBus.Publish(new CreatorRegisteredIntegrationEvent()
            {
                UserId = user.Id,
                Email = user.Email,
                Username = user.Username,
            });

            await SendOkAsync(ct);
            return;
        }
    }
}
