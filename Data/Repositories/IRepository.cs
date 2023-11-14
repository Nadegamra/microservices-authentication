namespace Authentication.Data.Repositories
{
    public abstract class IRepository<T> where T : class
    {
        protected readonly AuthDbContext dbContext;

        protected IRepository(AuthDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public abstract T? Get(int id);
        public abstract IQueryable<T> GetAll();
        public virtual T Add(T entity)
        {
            var res = dbContext.Add(entity);
            dbContext.SaveChanges();
            return (T)res.Entity;
        }
        public T Update(T entity)
        {
            var res = dbContext.Update(entity);
            dbContext.SaveChanges();
            return (T)res.Entity;
        }
        public T Delete(T entity)
        {
            var res = dbContext.Update(entity);
            dbContext.SaveChanges();
            return (T)res.Entity;
        }
    }
}