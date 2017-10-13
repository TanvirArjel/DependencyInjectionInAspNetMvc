using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ContosoUniversity.DAL
{
    public interface IRepository<TEntity> where TEntity: class 
    { 
        IQueryable<TEntity> GelAllEntities(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        TEntity GetById(object id);
        void InsertEntity(TEntity entity);
        void UpdateEntity(TEntity entity);
        void DeleteEntity(object id);
        void Save();
        void Dispose();

    }
}