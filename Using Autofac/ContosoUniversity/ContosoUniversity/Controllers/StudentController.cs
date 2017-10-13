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
using PagedList;
using PagedList.Mvc;

namespace ContosoUniversity.Controllers
{
    public class StudentController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public StudentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Student
        public ActionResult Index(string sortOrder, string searchString, int? page)
        {
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "Name_Desc" : ""; // Here the blank string means Name_Asc which is Default Sort
            ViewBag.DateSortParam = sortOrder == "Date_Asc" ? "Date_Desc" : "Date_Asc";

            IQueryable<Student> students = _unitOfWork.Repository<Student>().GelAllEntities();

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(x => x.FirstName.Contains(searchString) || x.LastName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name_Desc":
                    students = students.OrderByDescending(s => s.FirstName);
                    break;
                case "Date_Asc":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "Date_Desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.FirstName);
                    break;
            }
            return View(students.ToList().ToPagedList(page??1,3));
        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = _unitOfWork.Repository<Student>().GetById(id);
       
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.Repository<Student>().InsertEntity(student);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
                       
            return View(student);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = _unitOfWork.Repository<Student>().GetById(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Student studentToUpdate = _unitOfWork.Repository<Student>().GetById(id);
            if (TryUpdateModel(studentToUpdate, "", new string[] { "FirstName", "LastName", "EnrollmentDate" }))
            {
                try
                {
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(studentToUpdate);
        }

        // GET: Student/Delete/5
        public  ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Student student = _unitOfWork.Repository<Student>().GetById(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                _unitOfWork.Repository<Student>().DeleteEntity(id);
                _unitOfWork.Save();
            }
            catch (Exception)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Repository<Student>().Dispose();
            base.Dispose(disposing);
        }

    }
}
