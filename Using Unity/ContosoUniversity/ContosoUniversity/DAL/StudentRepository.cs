//using ContosoUniversity.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Data.Entity;

//namespace ContosoUniversity.DAL
//{
//    public class StudentRepository : IRepository, IDisposable
//    {
//        private SchoolContext context;
//        public StudentRepository(SchoolContext dbContext)
//        {
//            context = dbContext;
//        }

//        private SchoolContext context = new SchoolContext();

//        public IQueryable<TEntity> GelAllStudents()
//        {
//            return context.Students;
//        }

//        public TEntity GetById(int? id)
//        {
//            return context.Students.Find(id);
//        }

//        public void InsertStudent(TEntity student)
//        {
//            context.Students.Add(student);
//        }

//        public void UpdateStudent(TEntity student)
//        {
//            context.Entry(student).State = EntityState.Modified;
//        }

//        public void DeleteStudent(int studentID)
//        {
//            TEntity student = context.Students.Find(studentID);
//            context.Students.Remove(student);
//        }

//        public void Save()
//        {
//            context.SaveChanges();
//        }

//        private bool disposed = false;
//        protected virtual void Dispose(bool disposing)
//        {
//            if (!this.disposed)
//            {
//                if (disposing)
//                {
//                    context.Dispose();
//                }
//            }
//            this.disposed = true;
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//    }
//}