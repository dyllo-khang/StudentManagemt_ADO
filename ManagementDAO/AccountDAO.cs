using BusinessObject.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ManagementDAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;
        public static AccountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountDAO();
                }
                return instance;
            }
        }

        public Account GetAccountByEmail(string email)
        {
            Account account = null;
            using(SqlConnection conn = StudentManagementContext.Instance.CreateConnection())
            {
                conn.Open();
                string sql = "Select * from Account Where Email = @value";
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@value", email);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        account = new Account();
                        account.Email = reader["Email"].ToString();
                        account.FullName = reader["FullName"].ToString();
                        account.Password = reader["Password"].ToString();
                        account.Role = int.Parse(reader["Role"].ToString());
                    }
                }
            }
            return account;
        }

        public bool CheckExistData(string email)
        {
            using (SqlConnection conn = StudentManagementContext.Instance.CreateConnection())
            {
                conn.Open();
                string query = @"Select COUNT(1) from Account Where Email = @email";
                using(SqlCommand command = new SqlCommand(query, conn)) 
                {
                    command.Parameters.AddWithValue("@email", email);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if(count <= 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool AddAccount(Account account)
        {
            using(SqlConnection connection = StudentManagementContext.Instance.CreateConnection())
            {
                connection.Open();
                string query = @"Insert into Account Values(@email, @password, @fullName, @role)";
                using(SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddRange(new SqlParameter[]
                    {
                        new SqlParameter("@email", SqlDbType.NVarChar, 50) {Value = account.Email},
                        new SqlParameter("@password", SqlDbType.NVarChar, 20) {Value = account.Password},
                        new SqlParameter("@fullName", SqlDbType.NVarChar, 50) {Value = account.FullName},
                        new SqlParameter("@role", SqlDbType.Int) {Value = account.Role}
                    });
                    int result = cmd.ExecuteNonQuery();
                    if(result <=  0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
