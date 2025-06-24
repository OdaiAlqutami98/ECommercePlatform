using System.Linq.Expressions;
using Odai.Domain.Common;
using Odai.Shared.Table;

namespace ECommercePlatform.Logic.ICommonService
{
    public interface IBaseService<TEntity> where TEntity:BaseEntity
    {
        Task<TEntity?> GetById(int id);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetByDynamicParams(Dictionary<string, object> filters);
        Task<TableResponse<TEntity>> GetWithPagination(int pageIndex, int pageSize, Expression<Func<TEntity, bool>>? predicate = null);
        Task Insert(TEntity entity);
        void Update(TEntity entity);
        Task Delete(TEntity entity);
        Task AddRange(List<TEntity> entities);
        Task SaveChangesAsync();
    }
}
