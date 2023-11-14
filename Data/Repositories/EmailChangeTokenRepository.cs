using Authentication.Models;

namespace Authentication.Data.Repositories
{
    public class EmailChangeTokenRepository : IRepository<EmailChangeToken>
    {
        public EmailChangeTokenRepository(AuthDbContext dbContext) : base(dbContext)
        {

        }
        public override EmailChangeToken? Get(int id)
        {
            return dbContext.EmailChangeTokens.Where(x => x.Id == id).FirstOrDefault();
        }

        public override IQueryable<EmailChangeToken> GetAll()
        {
            return dbContext.EmailChangeTokens;
        }
    }
}