using FastEndpoints;

namespace Authentication.Endpoints.Profile
{
    public class ProfileEndpoint : Endpoint<ProfileRequest, ProfileResponse, ProfileMapper>
    {
        public override void Configure()
        {
            Get("auth/profile");
        }

        private readonly AuthDbContext appDbContext;

        public ProfileEndpoint(AuthDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public override async Task HandleAsync(ProfileRequest req, CancellationToken ct)
        {
            var user = appDbContext.Users.Where(x => x.Id == req.UserId).First();

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
