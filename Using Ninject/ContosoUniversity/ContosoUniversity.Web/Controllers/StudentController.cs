using ContosoUniversity.Models;
using PagedList;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using ContosoUniversity.RepositoryLayer.UnitOfWork;
using ContosoUniversity.ServiceLayer.StudentService;

namespace ContosoUniversity.Web.Controllers
{
    public class StudentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentService _studentService;

        public StudentController(IUnitOfWork unitOfWork, IStudentService studentService)
        {
            _unitOfWork = unitOfWork;
            _studentService = studentService;
        }

        // GET: Student
        public ActionResult Index(string sortOrder, string searchString, int? page)
        {
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "Name_Desc" : ""; // Here the blank string means Name_Asc which is Default Sort
            ViewBag.DateSortParam = sortOrder == "Date_Asc" ? "Date_Desc" : "Date_Asc";

            IQueryable<Student> students = _unitOfWork.Repository<Student>().GetAllEntities();

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
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await _unitOfWork.Repository<Student>().GetByIdAsync(id);

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
        public async Task<ActionResult> Create([Bind(Include = "FirstName,LastName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _studentService.CreateStudentAsync(student);
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
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await _unitOfWork.Repository<Student>().GetByIdAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Student studentToUpdate = await _unitOfWork.Repository<Student>().GetByIdAsync(id);
            if (TryUpdateModel(studentToUpdate, "", new string[] { "FirstName", "LastName", "EnrollmentDate" }))
            {
                try
                {
                    await _unitOfWork.SaveChangesAsync();
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
        public async Task<ActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Student student = await _unitOfWork.Repository<Student>().GetByIdAsync(id);
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
                _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            
            return RedirectToAction("Index");
        }

        

    }
}
