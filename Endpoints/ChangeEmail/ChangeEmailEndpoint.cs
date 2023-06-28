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
        private readonly AuthDbContext appDbContext;
        private readonly Infrastructure.EventBus.Generic.IEventBus eventBus;

        public ChangeEmailEndpoint(AuthDbContext appDbContext, Infrastructure.EventBus.Generic.IEventBus eventBus)
        {
            this.appDbContext = appDbContext;
            this.eventBus = eventBus;
        }

        public override async Task HandleAsync(ChangeEmailRequest req, CancellationToken ct)
        {
            EmailChangeToken? token = appDbContext.EmailChangeTokens.Where(x => x.Token == req.Token).FirstOrDefault();
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

            string oldEmail = user.Email;
            string oldUsername = user.Username;

            user.Email = token.EmailAddress;
            user.NormalizedEmail = token.EmailAddress.ToUpper();

            appDbContext.Users.Update(user);

            eventBus.Publish(new UserEmailChangedIntegrationEvent()
            {
                NewEmail = token.EmailAddress,
                OldEmail = oldEmail,
                UserId = user.Id
            });

            appDbContext.EmailChangeTokens.Remove(token);

            appDbContext.SaveChanges();

            await SendOkAsync(ct);

        }
    }
}
