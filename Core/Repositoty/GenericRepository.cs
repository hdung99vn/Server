using Core.Entities;
using Core.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text;
using AutoMapper;

namespace Core.Repositoty
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _entity;
        private readonly IDistributedCache _cache;
        private readonly IMapper _mapper;
        private DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(3));
        public GenericRepository(DbContext context,IDistributedCache cache, IMapper mapper)
        {
            _context = context;
            _entity = context.Set<T>();
            _cache = cache; 
            _mapper = mapper;
        }
        public async Task Add(T entity)
        {
            await _entity.AddAsync(entity);
            await _context.BulkSaveChangesAsync();
            var dataToCache = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(entity));
            await _cache.SetAsync(entity.Id.ToString(), dataToCache, options);
        }

        public async Task Add(IEnumerable<T> entities)
        {
            await _context.BulkInsertAsync(entities);
            await _context.BulkSaveChangesAsync();
            foreach(var entity in entities)
            {
                var dataToCache = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(entity));
                await _cache.SetAsync(entity.Id.ToString(), dataToCache, options);
            }
        }

        public async Task Delete(Guid Id)
        {
            var entity = await _entity.FindAsync(Id);
            if (entity != null)
            {
                _entity.Remove(entity);
            }
            await _context.BulkSaveChangesAsync();
            await _cache.RemoveAsync(Id.ToString());
        }

        public async Task Delete(IEnumerable<Guid> Ids)
        {
            var entities = await _entity.Where(x => Ids.Contains(x.Id)).ToListAsync();
            await _context.BulkDeleteAsync<IEnumerable<T?>>(entities);
            await _context.BulkSaveChangesAsync();
            foreach(var id in Ids)
            {
                await _cache.RemoveAsync(id.ToString());
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _entity.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetById(Guid Id)
        {
            var data= await _cache.GetAsync(Id.ToString());
            if (data != null)
            {
                var result = JsonSerializer.Deserialize<T>(data);
                return _mapper.Map<T>(result);
            }
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
            await _cache.RemoveAsync(entity.Id.ToString());
            var dataToCache = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(entity));
            await _cache.SetAsync(entity.Id.ToString(), dataToCache, options);
        }

        public async Task Update(IEnumerable<T> entities)
        {
            await _context.BulkUpdateAsync(entities);
            await _context.BulkSaveChangesAsync();
            foreach(var entity in entities)
            {
                await _cache.RemoveAsync(entity.Id.ToString());
                var dataToCache = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(entity));
                await _cache.SetAsync(entity.Id.ToString(), dataToCache, options);
            }
        }
    }
}
