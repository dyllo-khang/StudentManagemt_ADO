using BusinessObject.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementDAO
{
    public class ClassDAO
    {
        private static ClassDAO instance;
        private ClassDAO() { }
        public static ClassDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClassDAO();
                }
                return instance;
            }
        }

        public List<Class> GetAll()
        {
            List<Class> classes = null;
            using (SqlConnection connect = StudentManagementContext.Instance.CreateConnection())
            {
                connect.Open();
                string sql = @"Select * from Class";
                using(SqlCommand cmd = new SqlCommand(sql,connect))
                {
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        classes = new List<Class>();
                        while (reader.Read())
                        {
                            classes.Add(new Class()
                            {
                                ClassId = reader["ClassID"].ToString(),
                                ClassName = reader["ClassName"].ToString(),
                                Description = reader["Description"].ToString(),
                            });
                        }
                    }
                }
            }
            return classes;
        }
    }
}
