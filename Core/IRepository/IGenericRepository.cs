using Core.Entities;
using System.Linq.Expressions;

namespace Core.IRepository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task Add(T entity);
        public Task Update(T entity);
        public Task Delete(Guid Id);
        public Task<IEnumerable<T>> GetAll();
        public Task<T> GetById(Guid Id);
        public IQueryable<T> Query(Expression<Func<T, bool>> predicate);
        public Task Add(IEnumerable<T> entities);
        public Task Update(IEnumerable<T> entities);
        public Task Delete(IEnumerable<Guid> Ids);
    }
}
