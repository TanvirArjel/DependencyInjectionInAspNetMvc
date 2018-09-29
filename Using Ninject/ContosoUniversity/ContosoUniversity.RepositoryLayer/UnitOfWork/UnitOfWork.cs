using ContosoUniversity.DataAccessLayer;
using ContosoUniversity.RepositoryLayer.Repository;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace ContosoUniversity.RepositoryLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SchoolContext _dbContext;
        private Hashtable _repositories;
        public UnitOfWork(SchoolContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public UnitOfWork()
        //{
        //    _dbContext = new SchoolContext();
        //}
        

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                        .MakeGenericType(typeof(T)), _dbContext);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}