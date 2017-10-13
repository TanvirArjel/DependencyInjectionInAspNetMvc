using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ContosoUniversity.DAL
{
    public class SchoolContext : DbContext, IDbContext
    {
        public SchoolContext() : base("SchoolContext")
        {
        }
        public DbSet<Person> People { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Course>()
             .HasMany(c => c.Instructors).WithMany(i => i.Courses)
             .Map(t => t.MapLeftKey("CourseID")
                 .MapRightKey("InstructorID")
                 .ToTable("CourseInstructor"));

            //modelBuilder.Entity<Student>()
            //    .HasMany(s => s.Courses).WithMany(c => c.Students)
            //    .Map(t => t.MapLeftKey("StudentID")
            //    .MapRightKey("CourseID")
            //    .ToTable("StudentCourse"));
        }

        public IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        //public override int SaveChanges()
        //{
        //    this.ApplyStateChanges();
        //    base.SaveChanges();
        //}
    }
}