using BusinessObject;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementService
{
    public interface IStudentService
    {
        DataSet SearchStudentByFullName(string fullName);
        DataSet GetAll();
        DataSet GetStudentScores();
        bool AddStudent(Student student);
        bool DeleteStudent(string id);
        bool UpdateStudent(Student student);
        bool checkExistData(string columnName, string argument, string? studentID = null);
        //DataRow GetDataRow(string columnName, string param);
    }
}
