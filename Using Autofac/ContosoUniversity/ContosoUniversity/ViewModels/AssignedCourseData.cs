using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoUniversity.ViewModels
{
    public class AssignedCourseData
    {
        public int CourseID { get; set; }
        public string CourseTitle { get; set; }
        public bool Assigned { get; set; }
    }
}