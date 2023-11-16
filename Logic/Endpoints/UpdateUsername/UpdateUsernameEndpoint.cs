using Authentication.Data.Repositories;
using Authentication.IntegrationEvents.Events;
using Authentication.Models;
using FastEndpoints;

namespace Authentication.Endpoints.UpdateUsername
{
    public class UpdateUsernameEndpoint : Endpoint<UpdateUsernameRequest>
    {
        public override void Configure()
        {
            Put("auth/updateUsername");
        }

        private readonly IRepository<User> userRepository;
        private readonly Infrastructure.EventBus.Generic.IEventBus eventBus;

        public UpdateUsernameEndpoint(Infrastructure.EventBus.Generic.IEventBus eventBus, IRepository<User> userRepository)
        {
            this.eventBus = eventBus;
            this.userRepository = userRepository;
        }

        public override async Task HandleAsync(UpdateUsernameRequest req, CancellationToken ct)
        {
            User? user = userRepository.Get(req.UserId);
            if (user == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var username = user.Username;

            user.Username = req.NewUsername;
            user.NormalizedUsername = req.NewUsername.ToUpper();

            userRepository.Update(user);

            eventBus.Publish(new UserNameChangedIntegrationEvent()
            {
                NewUserName = req.NewUsername,
                OldUserName = username,
                UserId = user.Id
            });

            await SendNoContentAsync(ct);
        }
    }
}
