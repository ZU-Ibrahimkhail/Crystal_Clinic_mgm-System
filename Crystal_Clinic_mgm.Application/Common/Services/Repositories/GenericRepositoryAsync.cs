using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using System.Linq.Expressions;
namespace Crystal_Clinic_Mgm.Application.Common.Services.Repositories
{
    public class GenericRepositoryAsync<TContext, TEntity> : IGenericRepositoryAsync<TContext, TEntity> where TEntity : class where TContext : DbContext
    {
        private readonly TContext Db_Context;
        private readonly IMessage _message;
        public GenericRepositoryAsync(TContext db_Context, IMessage message)
        {
            Db_Context = db_Context;
            _message = message;
        }
        public async Task<JsonResult> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                await Db_Context.Set<TEntity>().AddAsync(entity, cancellationToken);
                await Db_Context.SaveChangesAsync(cancellationToken);
                return _message.Saved();
            }
            catch (Exception ex)
            {
                return _message.InternalServerError(ex.ToString());
            }
        }
        public async Task<JsonResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                Db_Context.Entry(entity).State = EntityState.Modified;
                await Db_Context.SaveChangesAsync(cancellationToken);
                return _message.Update();
            }
            catch (Exception ex)
            {
                return _message.InternalServerError(ex.ToString());
            }
        }
        public void EditeAsync(TEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                Db_Context.Entry(entity).State = EntityState.Modified;
                Db_Context.SaveChanges();
            }
            catch (Exception)
            {
            }
        }
        public async Task<JsonResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                Db_Context.Entry(entity).State = EntityState.Modified;
                await Db_Context.SaveChangesAsync(cancellationToken);
                return _message.Delete();
            }
            catch (Exception ex)
            {
                return _message.InternalServerError(ex.ToString());
            }
        }
        public async Task<JsonResult> RemoveAsync(TEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                Db_Context.Set<TEntity>().Remove(entity);
                await Db_Context.SaveChangesAsync(cancellationToken);
                return _message.Remove();
            }
            catch (Exception ex)
            {
                return _message.InternalServerError(ex.ToString());
            }
        }
        public async Task<JsonResult> FindAsync(int Id)
        {
            try
            {
                var res = await Db_Context.Set<TEntity>().FindAsync(Id);
                if (res == null)
                {
                    return _message.RecordNotFound();
                }
                else
                {
                    return new JsonResult(res);
                }
            }
            catch (Exception ex)
            {
                return _message.InternalServerError(ex.ToString());
            }
        }
        public async Task<TEntity?> GetDetailAsync(int Id)
        {
            try
            {
                return await Db_Context.Set<TEntity>().FindAsync(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return Db_Context.Set<TEntity>().Where(expression).AsNoTracking();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, int pageIndex, int pagesize, out int total)
        {
            try
            {
                total = Db_Context.Set<TEntity>().Count(expression);
                return Db_Context.Set<TEntity>().Where(expression).Skip(pageIndex * pagesize).Take(pagesize)
                  .AsNoTracking();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<TEntity> FindByConditionWithTracking(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return Db_Context.Set<TEntity>()
                  .Where(expression);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveAsync(TEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                Db_Context.Entry(entity).State = EntityState.Added;
                Db_Context.Set<TEntity>().Add(entity);
                Db_Context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void MultiRecordRemoveRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken)
        {
            try
            {
                Db_Context.Set<TEntity>().RemoveRange(entity);
                Db_Context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return Db_Context.Set<TEntity>()
                  .Where(expression).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return Db_Context.Set<TEntity>()
                  .Any(expression);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task MultipleUpdateByCondation(Expression<Func<TEntity, bool>> expression, Action<TEntity> action, CancellationToken cancellationToken)
        {
            try
            {
                await Db_Context.Set<TEntity>().Where(expression).ForEachAsync(action);
                await Db_Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TEntity?> GetDetailAsync(object Id)
        {
            try
            {
                return await Db_Context.Set<TEntity>().FindAsync(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task AddRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken)
        {

            try
            {
                Db_Context.Set<TEntity>().AddRange(entity);
                Db_Context.SaveChanges();
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
