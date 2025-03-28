using Microsoft.EntityFrameworkCore;
using Odai.DataModel;
using Odai.Domain.Common;
using System.Linq.Expressions;

namespace Odai.Logic.Common
{
    public abstract class BaseManager<TEntity, DbContext> where TEntity : BaseEntity where DbContext:OdaiDbContext
    {
        public OdaiDbContext _context;
        public BaseManager(OdaiDbContext context)
        {
            _context= context;
        }
        public DbSet<TEntity> Entity
        {
            get { return _context.Set<TEntity>(); }
        }
        public virtual async Task<TEntity?> GetById(int id)
        {
            return await Entity.FindAsync(id);
        }
        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate != null)
            {
                return Entity.Where(predicate);
            }
            else
            {
                return Entity;
            }
        }
        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate != null)
            {
                return Entity.Where(predicate);
            }
            else
            {
                return Entity;
            }
        }

        public virtual async Task Add(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public virtual Task Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }
        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public virtual async Task AddRange(List<TEntity> entitys)
        {
            await _context.Set<TEntity>().AddRangeAsync(entitys);
        }
    }
}
