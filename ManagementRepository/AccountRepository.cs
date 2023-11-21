using BusinessObject.Models;
using ManagementDAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementRepository
{
    public class AccountRepository : IAccountRepository
    {
        public bool AddAccount(Account account) => AccountDAO.Instance.AddAccount(account);

        public bool CheckExistEmail(string email) => AccountDAO.Instance.CheckExistData(email);
       
        public Account GetAccountByEmail(string email) => AccountDAO.Instance.GetAccountByEmail(email);

    }
}
