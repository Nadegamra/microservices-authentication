using Authentication.Models;

namespace Authentication.Data.Repositories
{
    public class PasswordChangeTokenRepository : IRepository<PasswordChangeToken>
    {
        public PasswordChangeTokenRepository(AuthDbContext dbContext) : base(dbContext)
        {

        }
        public override PasswordChangeToken? Get(int id)
        {
            return dbContext.PasswordChangeTokens.Where(x => x.Id == id).FirstOrDefault();
        }

        public override IQueryable<PasswordChangeToken> GetAll()
        {
            return dbContext.PasswordChangeTokens;
        }
    }
}