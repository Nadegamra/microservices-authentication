using Authentication.Models;

namespace Authentication.Data.Repositories
{
    public class EmailConfirmationTokenRepository : IRepository<EmailConfirmationToken>
    {
        public EmailConfirmationTokenRepository(AuthDbContext dbContext) : base(dbContext)
        {

        }
        public override EmailConfirmationToken? Get(int id)
        {
            return dbContext.EmailConfirmationTokens.Where(x => x.Id == id).FirstOrDefault();
        }

        public override IQueryable<EmailConfirmationToken> GetAll()
        {
            return dbContext.EmailConfirmationTokens;
        }
    }
}