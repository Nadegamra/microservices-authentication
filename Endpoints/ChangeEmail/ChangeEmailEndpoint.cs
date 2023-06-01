using Authentication.Models;
using FastEndpoints;

namespace Authentication.Endpoints.ChangeEmail
{
    public class ChangeEmailEndpoint: Endpoint<ChangeEmailRequest, EmptyResponse>
    {
        public override void Configure()
        {
            Post("auth/changeEmail");
            AllowAnonymous();
        }
        public readonly AppDbContext appDbContext;

        public ChangeEmailEndpoint(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public override async Task HandleAsync(ChangeEmailRequest req, CancellationToken ct)
        {
            EmailChangeToken? token = appDbContext.EmailChangeTokens.Where(x=>x.Token == req.Token).FirstOrDefault();
            if(token == null)
            {
                await SendErrorsAsync(400, ct);
                return;
            }

            var user = appDbContext.Users.Where(x=>x.Id == token.UserId).FirstOrDefault();
            if(user == null)
            {
                await SendErrorsAsync(400, ct);
                return;
            }

            user.Email = token.EmailAddress;
            user.NormalizedEmail = token.EmailAddress.ToUpper();

            user.Username = token.EmailAddress;
            user.NormalizedUsername = token.EmailAddress.ToUpper();

            appDbContext.Users.Update(user);

            appDbContext.EmailChangeTokens.Remove(token);

            appDbContext.SaveChanges();

            await SendOkAsync(ct);

        }
    }
}
