using ContosoUniversity.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ContosoUniversity.DataAccessLayer;

namespace ContosoUniversity.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly SchoolContext db = new SchoolContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            //Performing SQL Like Linq Query

            //IQueryable<EnrollmentDateGroup> data = from student in db.Students
            //                                       group student by student.EnrollmentDate into dateGroup
            //                                       select new EnrollmentDateGroup()
            //                                       {
            //                                           EnrollmentDate = dateGroup.Key,
            //                                           StudentCount = dateGroup.Count()
            //                                       };

            //Performing Linq query with Lamda Expression

            //IQueryable<EnrollmentDateGroup> data = db.Students.GroupBy(x => x.EnrollmentDate).Select(x => new EnrollmentDateGroup
            //                                        {
            //                                            EnrollmentDate = x.Key,
            //                                            StudentCount = x.Count()
            //                                        });

            //Performing Raw SQL query

            string query = "SELECT EnrollmentDate, COUNT(*) AS StudentCount FROM Person Where Discriminator = 'Student' Group By EnrollmentDate";

            IEnumerable<EnrollmentDateGroup> data = db.Database.SqlQuery<EnrollmentDateGroup>(query);

            return View(data.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}