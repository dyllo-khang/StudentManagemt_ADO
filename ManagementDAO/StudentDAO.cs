using BusinessObject;
using BusinessObject.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ManagementDAO
{
    public class StudentDAO
    {
        private static StudentDAO instance;
        private StudentDAO() { }
        public static StudentDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StudentDAO();
                }
                return instance;
            }
        }
        public DataSet GettAll()
        {
            DataSet ds = new DataSet();
            using (SqlConnection connect = StudentManagementContext.Instance.CreateConnection())
            {
                string sql = @"Select s.StudentId, s.FullName, Format(s.DateOfBirth, 'MM-dd-yyyy') as DateOfBirth, s.Gender, s.Address, s.Email, s.ClassId, c.ClassName From Student s Join Class c on s.ClassID = c.ClassID";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connect);
                adapter.Fill(ds);
            }
            return ds;
        }

        public DataSet SearchStudentByFullName(string fullName)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = StudentManagementContext.Instance.CreateConnection())
            {
                conn.Open();
                string sql = @"Select s.StudentId, s.FullName, Format(s.DateOfBirth, 'MM-dd-yyyy') as DateOfBirth, s.Gender, s.Address, s.Email, s.ClassId, c.ClassName From Student s Join Class c on s.ClassID = c.ClassID where s.FullName like @value";
                SqlCommand newQuery = new SqlCommand(sql, conn);
                newQuery.Parameters.AddWithValue("@value", "%" + fullName + "%");
                SqlDataAdapter adapter = new SqlDataAdapter(newQuery);
                adapter.Fill(ds);
            }
            return ds;
        }


        //public DataRow GetDataRow(string columnName, string param)
        //{
        //    DataTable dataTable = GettAll().Tables[0];
        //    return dataTable.AsEnumerable().FirstOrDefault(row => row.Field<string>(columnName).Equals(param));
        //}

        public bool CheckExistData(string columnName, string argument, string? studentID = null)
        {
            using(SqlConnection conn = StudentManagementContext.Instance.CreateConnection())
            {
                conn.Open();
                if(studentID == null)
                {
                    string query = @$"Select Count(1) From Student where {columnName} = @param";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@param", argument);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if(count <= 0) return false;
                    }
                }
                else
                {
                    string query = @$"Select Count(1) From Student where {columnName} = @param AND StudentID <> @studentID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@param", argument);
                        cmd.Parameters.AddWithValue("@studentID", studentID);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count <= 0) return false;
                    }
                }
            }
            return true;
        }
        public DataSet GetStudentScores()
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = StudentManagementContext.Instance.CreateConnection())
            {
                string sql = @"Select s.StudentID, s.FullName, c.ClassName,
                               (Select sc.ScoreValue from Score sc join Subject sb on sc.SubjectID = sb.SubjectID AND s.StudentID = sc.StudentID where sb.SubjectName = @value1) as MathScore,
                               (Select sc.ScoreValue from Score sc join Subject sb on sc.SubjectID = sb.SubjectID AND s.StudentID = sc.StudentID where sb.SubjectName = @value2) as PhysicalScore,
                               (Select sc.ScoreValue from Score sc join Subject sb on sc.SubjectID = sb.SubjectID AND s.StudentID = sc.StudentID where sb.SubjectName = @value3) as ChemistryScore,
                               (Select sc.ScoreValue from Score sc join Subject sb on sc.SubjectID = sb.SubjectID AND s.StudentID = sc.StudentID where sb.SubjectName = @value4) as EnglishScore
                               from Student s Join Class c on  s.ClassID = c.ClassID";
                SqlCommand newQuery = new SqlCommand(sql, connection);
                newQuery.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@value1", SqlDbType.NVarChar, 10) {Value = "Math"},
                    new SqlParameter("@value2", SqlDbType.NVarChar, 10) { Value = "Physical" },
                    new SqlParameter("@value3", SqlDbType.NVarChar, 10) { Value = "Chemistry" },
                    new SqlParameter("@value4", SqlDbType.NVarChar, 10) { Value = "English" }
                });
                SqlDataAdapter adapter = new SqlDataAdapter(newQuery);
                adapter.Fill(ds);
            }
            return ds;
        }
        public bool AddStudent(Student student)
        {
            bool isAdd = true;
            using (SqlConnection conn = StudentManagementContext.Instance.CreateConnection())
            {
                conn.Open();
                string sql = @"Insert into Student Values(@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(new SqlParameter[]
                    {
                        new SqlParameter("@value1", SqlDbType.NVarChar, 50) {Value = student.StudentId},
                        new SqlParameter("@value2", SqlDbType.NVarChar, 50) {Value = student.FullName},
                        new SqlParameter("@value3", SqlDbType.DateTime) {Value = student.DateOfBirth},
                        new SqlParameter("@value4", SqlDbType.NVarChar, 10) {Value = student.Gender},
                        new SqlParameter("@value5", SqlDbType.NVarChar, 50) {Value = student.Address},
                        new SqlParameter("@value6", SqlDbType.NVarChar, 50) {Value = student.Email},
                        new SqlParameter("@value7", SqlDbType.NVarChar, 10) {Value = student.ClassId}
                    });
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0) isAdd = false;
                }
            }
            return isAdd;
        }

        public bool DeleteStudent(string id)
        {
            using (SqlConnection connection = StudentManagementContext.Instance.CreateConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    string scoreSql = @"Delete from Score Where StudentID = @value";
                    using (SqlCommand cmd = new SqlCommand(scoreSql, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@value", id);
                        cmd.ExecuteNonQuery();
                    }
                    string studentSql = @"Delete from Student Where StudentID = @value";
                    using (SqlCommand cmd = new SqlCommand(studentSql, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@value", id);
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            transaction.Commit();
                            return true;
                        }
                        else
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public bool UpDateStudent(Student student)
        {
            using (SqlConnection connection = StudentManagementContext.Instance.CreateConnection())
            {
                connection.Open();
                string query = @"Update Student SET FullName = @fullName, DateOfBirth = @birthDay, Gender = @gender, Address = @address, Email = @email, ClassID = @classID where StudentID = @studentID";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddRange(new SqlParameter[]
                    {
                        new SqlParameter("@fullName", SqlDbType.NVarChar, 50) {Value = student.FullName},
                        new SqlParameter("@birthDay", SqlDbType.Date) {Value = student.DateOfBirth},
                        new SqlParameter("@gender", SqlDbType.NVarChar, 10) {Value = student.Gender},
                        new SqlParameter("@address", SqlDbType.NVarChar, 50) {Value = student.Address},
                        new SqlParameter("@email", SqlDbType.NVarChar, 50) {Value = student.Email},
                        new SqlParameter("@classID", SqlDbType.NVarChar, 10) {Value = student.ClassId},
                        new SqlParameter("@studentID", SqlDbType.NVarChar, 50) {Value = student.StudentId},
                    });
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
