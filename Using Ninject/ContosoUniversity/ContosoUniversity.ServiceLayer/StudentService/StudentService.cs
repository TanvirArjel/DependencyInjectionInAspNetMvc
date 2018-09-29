using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.RepositoryLayer.UnitOfWork;

namespace ContosoUniversity.ServiceLayer.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateStudentAsync(Student student)
        {
            _unitOfWork.Repository<Student>().InsertEntity(student);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
