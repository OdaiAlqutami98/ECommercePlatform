using Microsoft.EntityFrameworkCore;
using Odai.DataModel;
using Odai.Domain.Common;
using System.Linq.Expressions;

namespace Odai.Logic.Common
{
    public abstract class BaseServiceIdentity<TEntity, DbContext> where TEntity : class where DbContext:OdaiDbContext
    {
        public OdaiDbContext _context;
        public BaseServiceIdentity(OdaiDbContext context)
        {
            _context= context;
        }
        public DbSet<TEntity> Entity
        {
            get { return _context.Set<TEntity>(); }
        }
        public virtual async Task<TEntity?> GetById(Guid id)
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
        // GetParams
        public virtual IQueryable<TEntity> GetByDynamicParams(Dictionary<string, object> filters)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            Expression? finalExpression = null;

            foreach (var filter in filters)
            {
                var property = typeof(TEntity).GetProperty(filter.Key);
                if (property == null) continue;

                var member = Expression.Property(parameter, property);

                object? convertedValue;
                try
                {
                    convertedValue = Convert.ChangeType(filter.Value, property.PropertyType);
                }
                catch
                {
                    continue;
                }

                var constant = Expression.Constant(convertedValue, property.PropertyType);

                Expression comparison;
                if (property.PropertyType == typeof(string))
                {
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    comparison = Expression.Call(member, containsMethod!, constant);
                }
                else
                {
                    comparison = Expression.Equal(member, constant);
                }

                finalExpression = finalExpression == null ? comparison : Expression.AndAlso(finalExpression, comparison);
            }

            var lambda = Expression.Lambda<Func<TEntity, bool>>(finalExpression ?? Expression.Constant(true), parameter);
            return Entity.Where(lambda);
        }

        // GetPagination
        public virtual async Task<(IEnumerable<TEntity> Data, int TotalCount)> GetWithPagination(
            int pageIndex, int pageSize, Expression<Func<TEntity, bool>>? predicate = null)
        {
            var query = predicate != null ? Entity.Where(predicate) : Entity;

            int totalCount = await query.CountAsync();
            var data = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return (data, totalCount);
        }

        public virtual async Task Insert(TEntity entity)
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
        public virtual async Task AddRange(List<TEntity> entitys)
        {
            await _context.Set<TEntity>().AddRangeAsync(entitys);
        }
        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
