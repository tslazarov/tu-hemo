using System.Linq;

namespace Hemo.Data.Contracts
{
    public interface IEfRepository<T> where T : class
    {
        T GetById(object id);

        IQueryable<T> All();

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
