using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using System.Data.Entity.Infrastructure;
using ContosoUniversity.ViewModels;

namespace ContosoUniversity.Controllers
{
    public class InstructorController : Controller
    {
        private readonly SchoolContext db = new SchoolContext();

        // GET: Instructor
        public ActionResult Index( int? id, int? courseID)
        {
            var viewModel = new InstructorIndexData();
            viewModel.Instructors = db.Instructors
                                    .Include(i => i.OfficeAssignment)
                                    .Include(i => i.Courses)
                                    .OrderBy(i => i.FirstName);

            if (id != null)
            {
                ViewBag.InstructorID = id;
                viewModel.Courses = viewModel.Instructors.Single(i => i.ID == id).Courses;
            }

            if (courseID != null)
            {
                ViewBag.CourseID = courseID;
                viewModel.Enrollments = viewModel.Courses.Single(x => x.CourseID == courseID).Enrollments;
            }
            return View(viewModel);
        }

        // GET: Instructor/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = await db.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // GET: Instructor/Create
        public ActionResult Create()
        {
            var instructor = new Instructor();
            instructor.Courses = new List<Course>();
            PopulateAssignedCourseData(instructor);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "InstructorID,FirstName,LastName,HireDate,OfficeAssignment")] Instructor instructor, string[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                instructor.Courses = new List<Course>();
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = db.Courses.Find(int.Parse(course));
                    instructor.Courses.Add(courseToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                db.Instructors.Add(instructor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = await db.Instructors
                                            .Include(i => i.OfficeAssignment)
                                            .Include(i =>i.Courses)
                                            .Where(i => i.ID == id).SingleAsync();
            PopulateAssignedCourseData(instructor);
            if (instructor == null)
            {
                return HttpNotFound();
            }

            return View(instructor);
        }

        private void PopulateAssignedCourseData(Instructor instrcutor)
        {
            var allCourses = db.Courses;
            var instructorCourses = new HashSet<int>(instrcutor.Courses.Select(c => c.CourseID));
            var viewModel = new List<AssignedCourseData>();
            foreach ( var courses in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = courses.CourseID,
                    CourseTitle = courses.CourseTitle,
                    Assigned = instructorCourses.Contains(courses.CourseID)
                });
            }
            ViewBag.Courses = viewModel;
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var instructorToUpdate = db.Instructors
                                       .Include(i => i.OfficeAssignment)
                                       .Include(i => i.Courses) //ID = InstructorID
                                       .Single(i => i.ID == id);

            if (TryUpdateModel(instructorToUpdate, "",
               new string[] { "FirstName", "LastName", "HireDate", "OfficeAssignment" }))
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment.Location))
                    {
                        instructorToUpdate.OfficeAssignment = null;
                    }
                    UpdateInstructorCourses(selectedCourses, instructorToUpdate);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedCourseData(instructorToUpdate);
            return View(instructorToUpdate);
        }

        private void UpdateInstructorCourses(string [] selectedCoures, Instructor instructorToUpdate)
        {
            if (selectedCoures == null)
            {
                instructorToUpdate.Courses = new List<Course>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCoures);
            var instructorCourses = new HashSet<int>(instructorToUpdate.Courses.Select(c => c.CourseID));
            foreach (var course in db.Courses)
            {
                if (selectedCoursesHS.Contains(course.CourseID.ToString()))
                {
                    if (!instructorCourses.Contains(course.CourseID))
                    {
                        instructorToUpdate.Courses.Add(course);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.CourseID))
                    {
                        instructorToUpdate.Courses.Remove(course);
                    }
                }
            }
        }

        // GET: Instructor/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = await db.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Instructor instructor = await db.Instructors
                                          .Include(i => i.OfficeAssignment)
                                          .Where(i => i.ID == id)
                                          .SingleAsync();
            db.Instructors.Remove(instructor);

            var department = db.Departments.SingleOrDefault(d => d.InstructorID == id);
            if (department != null)
            {
                department.InstructorID = null;
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
