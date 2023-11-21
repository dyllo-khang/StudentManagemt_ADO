using BusinessObject;
using BusinessObject.Models;
using ManagementRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementService
{
    public class StudentService : IStudentService
    {
        private IStudentRepository _studentRepository;

        public StudentService()
        {
            _studentRepository = new StudentRepository();
        }

        public bool AddStudent(Student student) => _studentRepository.AddStudent(student);

        public bool checkExistData(string columnName, string argument, string? studentID = null) => _studentRepository.checkExistData(columnName, argument, studentID);

        public bool DeleteStudent(string id) => _studentRepository.DeleteStudent(id);

        public DataSet GetAll() => _studentRepository.GetAll();

        //public DataRow GetDataRow(string columnName, string param) => _studentRepository.GetDataRow(columnName, param);

        public DataSet GetStudentScores() => _studentRepository.GetStudentScores();

        public DataSet SearchStudentByFullName(string fullName) => _studentRepository.SearchStudentByFullName(fullName);

        public bool UpdateStudent(Student student) => _studentRepository.UpdateStudent(student);
    }
}
