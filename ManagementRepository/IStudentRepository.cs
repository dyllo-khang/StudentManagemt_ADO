using BusinessObject;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementRepository
{
    public interface IStudentRepository
    {
        DataSet SearchStudentByFullName(string fullName);
        DataSet GetAll();
        DataSet GetStudentScores();
        //DataRow GetDataRow(string columnName, string param);
        bool AddStudent(Student student);
        bool checkExistData(string columnName,string argument, string? studentID = null);
        bool DeleteStudent(string id);
        bool UpdateStudent(Student student);
    }
}
