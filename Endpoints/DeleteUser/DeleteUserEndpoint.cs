using Authentication.Models;
using FastEndpoints;

namespace Authentication.Endpoints.DeleteUser
{
    public class DeleteUserEndpoint : Endpoint<DeleteUserRequest>
    {
        public override void Configure()
        {
            Delete("auth/deleteUser");
        }

        private readonly AuthDbContext authDbContext;

        public DeleteUserEndpoint(AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }

        public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
        {
            User? user = authDbContext.Users.FirstOrDefault(x => x.Id == req.UserId);

            if (user == null)
            {
                await SendErrorsAsync(404, ct);
                return;
            }

            var userRoles = authDbContext.UserRoles.Where(x => x.UserId == req.UserId).Select(x => x.RoleId).ToList();
            var roleNames = authDbContext.Roles.Where(x => userRoles.Contains(x.Id)).Select(x => x.Name).ToList();
            if (roleNames.Contains("Admin"))
            {
                var adminCount = authDbContext.UserRoles.Where(x => x.RoleId == 1).Count();
                if (adminCount == 1)
                {
                    AddError("Cannot delete last admin");
                    await SendNotFoundAsync(ct);
                    return;
                }
            }

            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            authDbContext.Users.Update(user);

            List<UserToken> userTokens = authDbContext.UserTokens.Where(x => x.UserId == req.UserId).ToList();
            authDbContext.UserTokens.RemoveRange(userTokens);
            await authDbContext.SaveChangesAsync(ct);

            await SendOkAsync(ct);
        }
    }
}