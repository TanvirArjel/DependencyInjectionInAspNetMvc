using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoUniversity.DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly SchoolContext _dbContext = new SchoolContext();
        private Repository<Course> _courseRepository;
        private Repository<Department> _departmentRepository;
        private Repository<Student> _studentRepository;
        private Repository<Instructor> _instructorRepository;

        public Repository<Course> CourseRepository => _courseRepository ?? (_courseRepository = new Repository<Course>(_dbContext));
        public Repository<Department> DepartmentRepository => _departmentRepository ?? (_departmentRepository = new Repository<Department>(_dbContext));
        public Repository<Student> StudentRepository => _studentRepository ?? (_studentRepository = new Repository<Student>(_dbContext));
        public Repository<Instructor> InstructorRepository => _instructorRepository ?? (_instructorRepository = new Repository<Instructor>(_dbContext));
        
        public void Save()
        {
            _dbContext.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
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