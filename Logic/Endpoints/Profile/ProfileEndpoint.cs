using Authentication.Data.Repositories;
using Authentication.Models;
using FastEndpoints;

namespace Authentication.Endpoints.Profile
{
    public class ProfileEndpoint : Endpoint<ProfileRequest, ProfileResponse, ProfileMapper>
    {
        public override void Configure()
        {
            Get("auth/profile");
        }

        private readonly IRepository<User> repository;

        public ProfileEndpoint(IRepository<User> repository)
        {
            this.repository = repository;
        }

        public override async Task HandleAsync(ProfileRequest req, CancellationToken ct)
        {
            var user = repository.Get(req.UserId);

            if (user is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            Response = Map.FromEntity(user);
            await SendOkAsync(Response, ct);
        }
    }
}
