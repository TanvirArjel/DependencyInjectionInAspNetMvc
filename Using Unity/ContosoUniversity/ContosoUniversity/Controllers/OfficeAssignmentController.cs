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
    public class OfficeAssignmentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: OfficeAssignment
        public async Task<ActionResult> Index()
        {
            var officeAssignments = db.OfficeAssignments.Include(o => o.Instructor);
            return View(await officeAssignments.ToListAsync());
        }

        // GET: OfficeAssignment/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OfficeAssignment officeAssignment = await db.OfficeAssignments.FindAsync(id);
            if (officeAssignment == null)
            {
                return HttpNotFound();
            }
            return View(officeAssignment);
        }

        // GET: OfficeAssignment/Create
        public ActionResult Create()
        {
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FirstName");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "InstructorID,Location")] OfficeAssignment officeAssignment)
        {
            if (ModelState.IsValid)
            {
                db.OfficeAssignments.Add(officeAssignment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.InstructorID = new SelectList(db.Instructors, "InstructorID", "FirstName", officeAssignment.InstructorID);
            return View(officeAssignment);
        }

        // GET: OfficeAssignment/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OfficeAssignment officeAssignment = await db.OfficeAssignments.FindAsync(id);
            if (officeAssignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorID = new SelectList(db.Instructors, "InstructorID", "FirstName", officeAssignment.InstructorID);
            return View(officeAssignment);
        }

        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "InstructorID,Location")] OfficeAssignment officeAssignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(officeAssignment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.InstructorID = new SelectList(db.Instructors, "InstructorID", "FirstName", officeAssignment.InstructorID);
            return View(officeAssignment);
        }

        // GET: OfficeAssignment/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OfficeAssignment officeAssignment = await db.OfficeAssignments.FindAsync(id);
            if (officeAssignment == null)
            {
                return HttpNotFound();
            }
            return View(officeAssignment);
        }

        // POST: OfficeAssignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OfficeAssignment officeAssignment = await db.OfficeAssignments.FindAsync(id);
            db.OfficeAssignments.Remove(officeAssignment);
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
