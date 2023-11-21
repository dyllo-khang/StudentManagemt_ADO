using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementDAO
{
    public class SubjectDAO
    {
        private static SubjectDAO instance;
        private SubjectDAO() { }
        public static SubjectDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SubjectDAO();
                }
                return instance;
            }
        }
        public string GetSubjectIDByName(string subjectName)
        {
            string? subjectID = null;
            using(SqlConnection con = StudentManagementContext.Instance.CreateConnection())
            {
                con.Open();
                string sql = @"Select SubjectID from Subject sb where sb.SubjectName = @value";
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@value", subjectName);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        subjectID = result?.ToString();
                    }
                }
            }
            return subjectID;
        }
    }
}
