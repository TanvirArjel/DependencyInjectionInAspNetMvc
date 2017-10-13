using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using System.Data.Entity.Infrastructure;

namespace ContosoUniversity.Controllers
{
    public class CourseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: Course
        public ActionResult Index(int? selectedDepartment)
        {

            DepartmentDropDownList(selectedDepartment);
            //IEnumerable<Course> courses = unitOfWork.CourseRepository.GetAllEntity(c => !selectedDepartment.HasValue || c.DepartmentID == selectedDepartment, includeProperties: "Department");
            IEnumerable<Course> courses = _unitOfWork.Repository<Course>().GelAllEntities().ToList();
            return View(courses);
        }

        // GET: Course/Details/5
        public  ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = _unitOfWork.Repository<Course>().GetById(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            DepartmentDropDownList();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,CourseTitle,Credits,DepartmentID")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.Repository<Course>().InsertEntity(course);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            DepartmentDropDownList();
            return View(course);
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = _unitOfWork.Repository<Course>().GetById(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            DepartmentDropDownList(course.DepartmentID);
            return View(course);
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var courseToUpdate = _unitOfWork.Repository<Course>().GetById(id);
            if (TryUpdateModel(courseToUpdate, "", new string[] { "CourseID", "CourseTitle", "Credits", "DepartmentID" }))
            {
                try
                {
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            DepartmentDropDownList(courseToUpdate.DepartmentID);
            return View(courseToUpdate);
        }
        private void DepartmentDropDownList(object selectedDeaparment = null)
        {
            IEnumerable<Department> departmentList = _unitOfWork.Repository<Department>().GelAllEntities(orderBy: q => q.OrderBy(d => d.DepartmentName));
            ViewBag.DepartmentID = new SelectList(departmentList, "DepartmentID", "DepartmentName", selectedDeaparment);
        }
        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = _unitOfWork.Repository<Course>().GetById(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _unitOfWork.Repository<Course>().DeleteEntity(id);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
