using Authentication.Data.Repositories;
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

        private readonly IRepository<User> userRepository;
        private readonly IRepository<Role> roleRepository;
        private readonly IRepository<UserToken> userTokenRepository;
        private readonly IRepository<UserRole> userRoleRepository;

        public DeleteUserEndpoint(IRepository<User> userRepository, IRepository<UserToken> userTokenRepository, IRepository<UserRole> userRoleRepository, IRepository<Role> roleRepository)
        {
            this.userRepository = userRepository;
            this.userTokenRepository = userTokenRepository;
            this.userRoleRepository = userRoleRepository;
            this.roleRepository = roleRepository;
        }

        public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
        {
            User? user = userRepository.Get(req.UserId);

            if (user == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var userRoles = userRoleRepository.GetAll().Where(x => x.UserId == req.UserId).Select(x => x.RoleId).ToList();
            var roleNames = roleRepository.GetAll().Where(x => userRoles.Contains(x.Id)).Select(x => x.Name.ToUpper()).ToList();
            if (roleNames.Contains("ADMIN"))
            {
                var adminCount = userRoleRepository.GetAll().Where(x => x.RoleId == 1).Count();
                if (adminCount == 1)
                {
                    await SendForbiddenAsync(ct);
                    return;
                }
            }

            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            userRepository.Update(user);

            List<UserToken> userTokens = userTokenRepository.GetAll().Where(x => x.UserId == req.UserId).ToList();
            userTokens.ForEach(x => userTokenRepository.Delete(x));

            await SendNoContentAsync(ct);
        }
    }
}