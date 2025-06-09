using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.Common.Services.IRepositories
{
    public interface IGenericRepositoryAsync<TContext, TEntity> where TEntity : class where TContext : DbContext
    {
        Task<JsonResult> AddAsync(TEntity entity, CancellationToken cancellationToken);
        Task<JsonResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        void EditeAsync(TEntity entity, CancellationToken cancellationToken);

        Task<JsonResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken);
        Task<JsonResult> RemoveAsync(TEntity entity, CancellationToken cancellationToken);
        Task<JsonResult> FindAsync(int Id);
        Task<TEntity?> GetDetailAsync(int Id);

        void SaveAsync(TEntity entity, CancellationToken cancellationToken);
        Task MultipleUpdateByCondation(Expression<Func<TEntity, bool>> expression, Action<TEntity> action, CancellationToken cancellationToken);
        void MultiRecordRemoveRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken);
        //   Task<IReadOnlyList<TEntity>> ListAllAsync();
        bool Any(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, int pageIndex, int pagesize, out int total);
        IQueryable<TEntity> FindByConditionWithTracking(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression);
        Task<TEntity?> GetDetailAsync(object iD);
        Task AddRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken);
        //   Task<IReadOnlyList<TEntity>> GetPagedReponseAsync(int page, int size);

    }
}
