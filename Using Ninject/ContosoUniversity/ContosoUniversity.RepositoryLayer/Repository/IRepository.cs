using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ContosoUniversity.RepositoryLayer.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAllEntities(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            /*string includeProperties = ""*/params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> GetByIdAsync(object id);
        void InsertEntity(TEntity entity);
        void UpdateEntity(TEntity entity, params string[] excludeProperties);
        void DeleteEntity(object id);
       

    }
}