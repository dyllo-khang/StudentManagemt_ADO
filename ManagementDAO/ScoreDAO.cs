using BusinessObject;
using BusinessObject.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementDAO
{
    public class ScoreDAO
    {
        private static ScoreDAO instance;
        private ScoreDAO() { }

        public static ScoreDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScoreDAO();
                }
                return instance;
            }
        }
        public bool isValidStudentScore(string studentID, string subjectID)
        {
            using (SqlConnection connection = StudentManagementContext.Instance.CreateConnection())
            {
                connection.Open();
                string sql = @"Select Count(1) from Score Where StudentID = @value1 AND SubjectID = @value2";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@value1", studentID);
                    cmd.Parameters.AddWithValue("@value2", subjectID);
                    int count = (int)cmd.ExecuteScalar();
                    if (count <= 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool UpdateScore(List<StudentScore> changedScores)
        {
            using (SqlConnection connection = StudentManagementContext.Instance.CreateConnection())
            {
                connection.Open();
                foreach (var score in changedScores)
                {
                    if (isValidStudentScore(score.StudentID, score.SubjectID))
                    {
                        if(score.Score < 0  || score.Score >10)
                        {
                            throw new ArgumentOutOfRangeException("Number in range 0-10");
                        }
                        string sql = @"Update Score set ScoreValue = @value1 where StudentID = @value2 AND SubjectID = @value3";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@value1", score.Score);
                            command.Parameters.AddWithValue("@value2", score.StudentID);
                            command.Parameters.AddWithValue("@value3", score.SubjectID);
                            int result = command.ExecuteNonQuery();
                            if (result <= 0)
                            {
                                connection.Close();
                                return false;
                            }
                        }
                    }
                    else
                    {
                        string sqlAddScore = @"Insert into Score VALUES(@value1, @value2, @value3)";
                        using (SqlCommand command = new SqlCommand(sqlAddScore, connection))
                        {                           
                            if (score.Score < 0 || score.Score > 10)
                            {
                                throw new ArgumentOutOfRangeException("Number in range 0-10");
                            }
                            command.Parameters.AddWithValue("@value1", score.StudentID);
                            command.Parameters.AddWithValue("@value2", score.SubjectID);
                            command.Parameters.AddWithValue("@value3", score.Score);
                            int result = command.ExecuteNonQuery();
                            if(result <= 0)
                            {
                                connection.Close();
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

    }
}
