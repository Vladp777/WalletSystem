using Domain.Entities;
using Domain.Enums;


namespace Application.Repositories
{
    internal interface ITransactionRepository: IBaseRepository<Transaction>
    {
        Task<bool> DeleteAllTransactions(Guid accountId);
        Task<IEnumerable<Transaction>> GetTransactionsByTag(Guid accountId, TransactionTag tag);
    }
}
