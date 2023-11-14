using Authentication.Models;

namespace Authentication.Data.Repositories
{
    public class UserRepository : IRepository<User>
    {
        public UserRepository(AuthDbContext dbContext) : base(dbContext)
        {

        }
        public override User? Get(int id)
        {
            return dbContext.Users.Where(x => x.Id == id).FirstOrDefault();
        }

        public override IQueryable<User> GetAll()
        {
            return dbContext.Users;
        }
    }
}