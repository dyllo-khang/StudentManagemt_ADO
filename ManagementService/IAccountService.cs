using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementService
{
    public interface IAccountService
    { 
        Account GetAccount(string email, string passWord);
        bool AddAccount(Account account);
        bool CheckExistEmail(string email);
    }
}
