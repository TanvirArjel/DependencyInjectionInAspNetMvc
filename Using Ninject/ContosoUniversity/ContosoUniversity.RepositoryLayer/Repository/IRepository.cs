using System;
using System.Linq;
using System.Linq.Expressions;

namespace ContosoUniversity.RepositoryLayer.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GelAllEntities(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        TEntity GetById(object id);
        void InsertEntity(TEntity entity);
        void UpdateEntity(TEntity entity);
        void DeleteEntity(object id);
       

    }
}