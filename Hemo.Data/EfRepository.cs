using Bytes2you.Validation;
using Hemo.Data.Contracts;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Hemo.Data
{
    public class EfRepository<T> : IEfRepository<T>
                   where T : class
    {
        private readonly IContext dbContext;
        private readonly IDbSet<T> dbSet;

        public EfRepository(IContext dbContext)
        {
            Guard.WhenArgument<IContext>(dbContext, "Database context cannot be null.")
                .IsNull()
                .Throw();

            this.dbContext = dbContext;
            this.dbSet = this.dbContext.Set<T>();
        }

        public IQueryable<T> All()
        {
            return this.dbSet;
        }

        public T GetById(object id)
        {
            return this.dbSet.Find(id);
        }

        public void Add(T entity)
        {
            DbEntityEntry entry = this.AttachEntry(entity);
            entry.State = EntityState.Added;
        }

        public void Delete(T entity)
        {
            DbEntityEntry entry = this.AttachEntry(entity);
            entry.State = EntityState.Deleted;
        }

        public void Update(T entity)
        {
            DbEntityEntry entry = this.AttachEntry(entity);
            entry.State = EntityState.Modified;
        }

        private DbEntityEntry AttachEntry(T entity)
        {
            DbEntityEntry entry = this.dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.dbSet.Attach(entity);
            }

            return entry;
        }
    }
}
