using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ECommercePlatform.Logic.ICommonService;
using Microsoft.EntityFrameworkCore;
using Odai.DataModel;
using Odai.Domain.Common;
using Odai.Shared.Table;

namespace ECommercePlatform.Logic.CommonService
{
   public class BaseService<TEntity> :IBaseService<TEntity> where TEntity:BaseEntity
    {
        protected readonly OdaiDbContext _context;

        public BaseService(OdaiDbContext context)
        {
            _context = context;
        }

        public DbSet<TEntity> Entity => _context.Set<TEntity>();

        public virtual async Task<TEntity?> GetById(int id) => await Entity.FirstOrDefaultAsync(e=>e.Id==id && e.IsDeleted==0);

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate) => Entity.Where(e=>e.IsDeleted==0).Where(predicate);

        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null)
            => predicate != null ? Entity.Where(e=>e.IsDeleted==0).Where(predicate) : Entity.Where(e => e.IsDeleted == 0);

        public virtual IQueryable<TEntity> GetByDynamicParams(Dictionary<string, object> filters)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            Expression finalExpression = Expression.Equal(Expression.Property(parameter, "IsDeleted"), Expression.Constant(0));

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
                catch { continue; }

                var constant = Expression.Constant(convertedValue, property.PropertyType);

                Expression comparison = property.PropertyType == typeof(string)
                    ? Expression.Call(member, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, constant)
                    : Expression.Equal(member, constant);

                finalExpression = Expression.AndAlso(finalExpression, comparison);
            }

            var lambda = Expression.Lambda<Func<TEntity, bool>>(finalExpression, parameter);
            return Entity.Where(lambda);
        }

        public virtual async Task<TableResponse<TEntity>> GetWithPagination(int pageIndex, int pageSize, Expression<Func<TEntity, bool>>? predicate = null)
        {
            

            var baseQuery = Entity.Where(e => e.IsDeleted == 0);
            var query = predicate != null ? baseQuery.Where(predicate) : baseQuery;
            int totalCount = await query.CountAsync();
            var data = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            return new TableResponse<TEntity>(data, totalCount);
        }

        public virtual async Task Insert(TEntity entity) => await Entity.AddAsync(entity);

        public virtual void Update(TEntity entity) => Entity.Update(entity);

        public virtual Task Delete(TEntity entity)
        {
            Entity.Remove(entity);
            return Task.CompletedTask;
        }

        public virtual async Task AddRange(List<TEntity> entities) => await Entity.AddRangeAsync(entities);

        public virtual async Task SaveChangesAsync() => await _context.SaveChangesAsync();

       
    }
}
