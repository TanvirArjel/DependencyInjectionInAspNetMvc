using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoUniversity.ServiceLayer.StudentService
{
    public interface IStudentService
    {
        Task CreateStudentAsync(Student student);
    }
}
