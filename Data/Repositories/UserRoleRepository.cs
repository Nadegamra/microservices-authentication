using Authentication.Models;

namespace Authentication.Data.Repositories
{
    public class UserRoleRepository : IRepository<UserRole>
    {
        public UserRoleRepository(AuthDbContext dbContext) : base(dbContext)
        {

        }
        public override UserRole? Get(int id)
        {
            return dbContext.UserRoles.Where(x => x.UserId == id).FirstOrDefault();
        }

        public override IQueryable<UserRole> GetAll()
        {
            return dbContext.UserRoles;
        }
    }
}