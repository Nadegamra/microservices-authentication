using Authentication.Models;

namespace Authentication.Data.Repositories
{
    public class RoleRepository : IRepository<Role>
    {
        public RoleRepository(AuthDbContext dbContext) : base(dbContext)
        {

        }
        public override Role? Get(int id)
        {
            return dbContext.Roles.Where(x => x.Id == id).FirstOrDefault();
        }

        public override IQueryable<Role> GetAll()
        {
            return dbContext.Roles;
        }
    }
}