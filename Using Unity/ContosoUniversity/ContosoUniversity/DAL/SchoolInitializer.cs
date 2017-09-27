using System;
using System.Collections.Generic;
using System.Data.Entity;
using ContosoUniversity.Models;

namespace ContosoUniversity.DAL
{
    public class SchoolInitializer : DropCreateDatabaseIfModelChanges<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            List<Student> students = new List<Student>
            {
            new Student{FirstName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01")},
            new Student{FirstName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2003-09-01")},
            new Student{FirstName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2001-09-01")},
            new Student{FirstName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2003-09-01")},
            new Student{FirstName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2005-09-01")}
            };
            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();

            List<Course> courses = new List<Course>
            {
            new Course{CourseID=1050,CourseTitle="Chemistry",Credits=3,},
            new Course{CourseID=4022,CourseTitle="Microeconomics",Credits=3,},
            new Course{CourseID=4041,CourseTitle="Macroeconomics",Credits=3,},
            new Course{CourseID=1045,CourseTitle="Calculus",Credits=4,},
            new Course{CourseID=3141,CourseTitle="Trigonometry",Credits=4,},
            new Course{CourseID=2021,CourseTitle="Composition",Credits=3,},
            new Course{CourseID=2042,CourseTitle="Literature",Credits=4,}
            };
            courses.ForEach(s => context.Courses.Add(s));
            context.SaveChanges();


            //List<Enrollment> enrollments = new List<Enrollment>
            //{
            //new Enrollment{StudentID=1,CourseID=1050,EnrollmentGrade = Enrollment.Grade.A},
            //new Enrollment{StudentID=1,CourseID=4022,EnrollmentGrade = Enrollment.Grade.C},
            //new Enrollment{StudentID=1,CourseID=4041,EnrollmentGrade = Enrollment.Grade.B},
            //new Enrollment{StudentID=2,CourseID=1045,EnrollmentGrade = Enrollment.Grade.B},
            //new Enrollment{StudentID=2,CourseID=3141,EnrollmentGrade = Enrollment.Grade.F},
            //new Enrollment{StudentID=2,CourseID=2021,EnrollmentGrade = Enrollment.Grade.F},
            //new Enrollment{StudentID=3,CourseID=1050},
            //new Enrollment{StudentID=4,CourseID=1050,},
            //new Enrollment{StudentID=4,CourseID=4022,EnrollmentGrade = Enrollment.Grade.F},
            //new Enrollment{StudentID=5,CourseID=4041,EnrollmentGrade = Enrollment.Grade.C},
            //new Enrollment{StudentID=6,CourseID=1045},
            //new Enrollment{StudentID=7,CourseID=3141,EnrollmentGrade = Enrollment.Grade.A},
            //};
            //enrollments.ForEach(s => context.Enrollments.Add(s));
            //context.SaveChanges();
        }
    }
}