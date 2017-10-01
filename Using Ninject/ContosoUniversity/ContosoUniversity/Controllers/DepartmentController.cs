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

namespace ContosoUniversity.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<Instructor> _instructorRepository;
        
        public DepartmentController(IRepository<Department> departmentRepository, IRepository<Instructor> instructorRepository )
        {
            _departmentRepository = departmentRepository;
            _instructorRepository = instructorRepository;
        }

        // GET: Department
        public ActionResult Index()
        {
            //var departments = db.Departments.Include(d => d.Administrator);
            IQueryable<Department> departments = _departmentRepository.GelAllEntities();
            //IEnumerable<Department> departments = _departmentRepository.GetAllEntity(/*includeProperties: "Administrator"*/);
            return View(departments.ToList());
        }

        // GET: Department/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = _departmentRepository.GetById(id);

            //string query = "SELECT * FROM Department WHERE DepartmentID = @p0";
            //Department department = await db.Departments.SqlQuery(query, id).SingleOrDefaultAsync();
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            AdministratorDropDownList();
            return View();
        }

        public void AdministratorDropDownList(object selectedAdministator = null)
        {
            IEnumerable<Instructor> instructors = _instructorRepository.GelAllEntities(/*orderBy: q => q.OrderBy(x => x.FirstName)*/);
            ViewBag.InstructorList = new SelectList(instructors, "ID", "FirstName", selectedAdministator);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartmentID,DepartmentName,Budget,StartDate,InstructorID")] Department department)
        {
            if (ModelState.IsValid)
            {
                _departmentRepository.InsertEntity(department);
                _departmentRepository.Save();
                return RedirectToAction("Index");
            }
            AdministratorDropDownList(department.InstructorID);
            return View(department);
        }

        // GET: Department/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = _departmentRepository.GetById(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            AdministratorDropDownList(department.InstructorID);
            return View(department);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentID,DepartmentName,Budget,StartDate,InstructorID")] Department department)
        {
            if (ModelState.IsValid)
            {
                _departmentRepository.UpdateEntity(department);
                _departmentRepository.Save();
                return RedirectToAction("Index");
            }
            AdministratorDropDownList(department.InstructorID);
            return View(department);
        }

        // GET: Department/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = _departmentRepository.GetById(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _departmentRepository.DeleteEntity(id);
            _departmentRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
           _departmentRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
