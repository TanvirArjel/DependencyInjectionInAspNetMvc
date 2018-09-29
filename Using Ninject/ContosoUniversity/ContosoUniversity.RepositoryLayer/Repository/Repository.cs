using ContosoUniversity.DataAccessLayer;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace ContosoUniversity.RepositoryLayer.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly SchoolContext _dbContext;
        private readonly IDbSet<TEntity> _dbSet;

        public Repository(SchoolContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GelAllEntities(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = ""
        )
        {
            IQueryable<TEntity> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (
                var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

       

        public TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public void InsertEntity(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void UpdateEntity(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
        public void DeleteEntity(object id)
        {
            TEntity entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
            

        }

    }

}