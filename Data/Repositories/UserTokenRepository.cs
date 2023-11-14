using Authentication.Models;

namespace Authentication.Data.Repositories
{
    public class UserTokenRepository : IRepository<UserToken>
    {
        public UserTokenRepository(AuthDbContext dbContext) : base(dbContext)
        {

        }
        public override UserToken? Get(int id)
        {
            return dbContext.UserTokens.Where(x => x.Id == id).FirstOrDefault();
        }

        public override IQueryable<UserToken> GetAll()
        {
            return dbContext.UserTokens;
        }
    }
}