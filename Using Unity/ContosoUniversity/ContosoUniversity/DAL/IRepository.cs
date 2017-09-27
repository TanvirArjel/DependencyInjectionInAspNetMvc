using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoUniversity.DAL
{
    public interface IRepository<TEntity> where TEntity: class 
    { 
        IQueryable<TEntity> GelAllEntities();
        TEntity GetById(object id);
        void InsertEntity(TEntity entity);
        void UpdateEntity(TEntity entity);
        void DeleteEntity(object id);
        void Save();
        void Dispose();

    }
}