using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementDAO
{
    public class StudentManagementContext
    {
        private static StudentManagementContext instance;
        public StudentManagementContext() { }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
        public static StudentManagementContext Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StudentManagementContext();
                }
                return instance;
            }
        }
        protected string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            IConfiguration configuration = builder.Build();
            return configuration.GetConnectionString("DBDefault");
        }
    }
}
