using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContosoUniversity.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Course Number")]
        public int CourseID { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Course Title cannot be longer than 100 characters")]
        [Display(Name = "Course Title")]
        public string CourseTitle { get; set; }
        [Range(1,5)]
        public int Credits { get; set; }

        public int DepartmentID { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Instructor> Instructors { get; set; }
    }
}