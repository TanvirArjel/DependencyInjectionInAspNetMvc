using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContosoUniversity.Models
{
    public class Person
    {
        public int ID { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z ''-'\s]*$", ErrorMessage = "Name Should contain alphabetical character only")]
        [StringLength(40, ErrorMessage = "First name cannot be longer than 40 characters.")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Last name cannot be longer than 20 characters.")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Full Name")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}