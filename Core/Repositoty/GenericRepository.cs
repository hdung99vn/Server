using Core.Entities;
using Core.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Repositoty
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _entity;
        public GenericRepository(DbContext context)
        {
            _context = context;
            _entity = context.Set<T>();
        }
        public async Task Add(T entity)
        {
            await _entity.AddAsync(entity);
            await _context.BulkSaveChangesAsync();
        }

        public async Task Add(IEnumerable<T> entities)
        {
            await _context.BulkInsertAsync(entities);
            await _context.BulkSaveChangesAsync();
        }

        public async Task Delete(Guid Id)
        {
            var entity = await _entity.FindAsync(Id);
            if (entity != null)
            {
                _entity.Remove(entity);
            }
            await _context.BulkSaveChangesAsync();
        }

        public async Task Delete(IEnumerable<Guid> Ids)
        {
            var entities=await _entity.Where(x=>Ids.Contains(x.Id)).ToListAsync();
            await _context.BulkDeleteAsync<IEnumerable<T?>>(entities);
            await _context.BulkSaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _entity.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetById(Guid Id)
        {
            return await _entity.FirstAsync(x => x.Id == Id);
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return _entity.Where(predicate);
        }

        public async Task Update(T entity)
        {
            _entity.Update(entity);
            await _context.BulkSaveChangesAsync();
        }

        public async Task Update(IEnumerable<T> entities)
        {
            await _context.BulkUpdateAsync(entities);
            await _context.BulkSaveChangesAsync();
        }
    }
}
