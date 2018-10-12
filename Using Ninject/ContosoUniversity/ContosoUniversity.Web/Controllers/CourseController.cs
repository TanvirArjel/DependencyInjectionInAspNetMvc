using ContosoUniversity.Models;
using ContosoUniversity.RepositoryLayer.UnitOfWork;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ContosoUniversity.Web.Controllers
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
            //IEnumerable<Course> courses = unitOfWork.Repository<Course>().GetAllEntity(c => !selectedDepartment.HasValue || c.DepartmentID == selectedDepartment, includeProperties: "Department");
            IEnumerable<Course> courses = _unitOfWork.Repository<Course>().GelAllEntities().ToList();
            return View(courses);
        }

        // GET: Course/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await _unitOfWork.Repository<Course>().GetByIdAsync(id);
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
                    _unitOfWork.SaveChangesAsync();
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
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await _unitOfWork.Repository<Course>().GetByIdAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            DepartmentDropDownList(course.DepartmentID);
            return View(course);
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var courseToUpdate = await _unitOfWork.Repository<Course>().GetByIdAsync(id);
            if (TryUpdateModel(courseToUpdate, "", new string[] { "CourseID", "CourseTitle", "Credits", "DepartmentID" }))
            {
                try
                {
                    await _unitOfWork.SaveChangesAsync();
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
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await _unitOfWork.Repository<Course>().GetByIdAsync(id);
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
            _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        
    }
}
