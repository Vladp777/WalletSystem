using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    internal interface IAccountRepository
    {
        Task<Account> CreateAccount(Account account);
        Task<Account> DeleteAccount(Account account);
        Task<IEnumerable<Account>> GetAllUserAccounts();
        Task<IEnumerable<Account>> GetAccountById(Guid id);
        Task<Account> ChangeBalance(Guid id);
        Task<Account> ChangeName(Guid id);

    }
}
