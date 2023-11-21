using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementRepository
{
    public interface IAccountRepository
    {
        Account GetAccountByEmail(string email);
        bool AddAccount(Account account);
        bool CheckExistEmail(string email);
    }
}
