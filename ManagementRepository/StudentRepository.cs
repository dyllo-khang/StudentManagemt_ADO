using BusinessObject;
using BusinessObject.Models;
using ManagementDAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementRepository
{
    public class StudentRepository : IStudentRepository
    {
        public bool AddStudent(Student student) => StudentDAO.Instance.AddStudent(student);

        public bool checkExistData(string columnName, string argument, string? studentID = null) => StudentDAO.Instance.CheckExistData(columnName,argument, studentID);

        public bool DeleteStudent(string id) => StudentDAO.Instance.DeleteStudent(id);

        public DataSet GetAll() => StudentDAO.Instance.GettAll();

        //public DataRow GetDataRow(string columnName, string param) => StudentDAO.Instance.GetDataRow(columnName, param);

        public DataSet GetStudentScores() =>StudentDAO.Instance.GetStudentScores();

        public DataSet SearchStudentByFullName(string fullName) => StudentDAO.Instance.SearchStudentByFullName(fullName);
     
        public bool UpdateStudent(Student student) => StudentDAO.Instance.UpDateStudent(student);
    }
}
