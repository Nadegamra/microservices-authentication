using Authentication.IntegrationEvents.Events;
using Authentication.Models;
using FastEndpoints;
using YamlDotNet.Core.Tokens;

namespace Authentication.Endpoints.UpdateUsername
{
    public class UpdateUsernameEndpoint: Endpoint<UpdateUsernameRequest>
    {
        public override void Configure()
        {
            Put("auth/updateUsername");
        }

        private readonly AuthDbContext authDbContext;
        private readonly Infrastructure.EventBus.Generic.IEventBus eventBus;

        public UpdateUsernameEndpoint(AuthDbContext authDbContext, Infrastructure.EventBus.Generic.IEventBus eventBus)
        {
            this.authDbContext = authDbContext;
            this.eventBus = eventBus;
        }

        public override async Task HandleAsync(UpdateUsernameRequest req, CancellationToken ct)
        {
            User? user = authDbContext.Users.Where(x=>x.Id == req.UserId).FirstOrDefault();
            if(user == null)
            {
                await SendErrorsAsync(400);
                return;
            }

            var username = user.Username;

            user.Username = req.NewUsername;
            user.NormalizedUsername = req.NewUsername.ToUpper();

            authDbContext.Users.Update(user);
            authDbContext.SaveChanges();

            eventBus.Publish(new UserNameChangedIntegrationEvent()
            {
                NewUserName = req.NewUsername,
                OldUserName = username,
                UserId = user.Id
            });

            await SendOkAsync(ct);
        }
    }
}
