using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ContosoUniversity.RepositoryLayer.Repository;

namespace ContosoUniversity.RepositoryLayer.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;
        Task SaveChangesAsync();

        // Commenting this because there is no need of explicit Disposing while using dependency injection because this the duty of DI container
        // to dispose the context
        //void Dispose(bool disposing);
        //void Dispose();  
    }
}