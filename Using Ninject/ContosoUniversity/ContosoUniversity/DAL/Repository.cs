using ContosoUniversity.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ContosoUniversity.DAL
{
    public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
        internal SchoolContext context;
        internal DbSet<TEntity> dbSet;

        public Repository(SchoolContext dbContext)
        {
            context = dbContext;
            dbSet = context.Set<TEntity>();
        }



       

        //public IQueryable<TEntity> GelAllEntities(
        //    Expression<Func<TEntity, bool>> filter = null,
        //    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        //    string includeProperties = ""
        //)
        //{
        //    IQueryable<TEntity> query = dbSet;
        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }

        //    foreach (
        //        var includeProperty in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
        //    {
        //        query = query.Include(includeProperty);
        //    }
        //    if (orderBy != null)
        //    {
        //        return orderBy(query);
        //    }
        //    else
        //    {
        //        return query;
        //    }
        //}
        public IQueryable<TEntity> GelAllEntities()
        {
            return dbSet;
        }
        public TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        public void InsertEntity(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void UpdateEntity(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
        public void DeleteEntity(object id)
        {
            TEntity entity = dbSet.Find(id);
            dbSet.Remove(entity);

        }
        public void Save()
        {
            context.SaveChanges();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }

}