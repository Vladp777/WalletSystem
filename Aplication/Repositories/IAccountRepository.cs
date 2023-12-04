using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface IAccountRepository: IBaseRepository<Account>
{
    Task<Account> NoTrackingGet(Guid id);
    Task<double> UpdateBalance(Guid id, double transactionCount, int typeId);
}
