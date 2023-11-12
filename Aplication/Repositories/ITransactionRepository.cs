using Domain.Entities;
using Domain.Enums;


namespace Application.Repositories
{
    internal interface ITransactionRepository
    {
        Task<Transaction> AddTransaction(Transaction transaction);
        Task<Transaction> DeleteTransaction(Guid id);
        Task<Transaction> UpdateTransaction(Transaction transaction);
        Task<IEnumerable<Transaction>> GetAllTransactions();
        Task<bool> DeleteAllTransactions();
        Task<IEnumerable<Transaction>> GetTransactionsByTag(Guid id, TransactionTag tag);
    }
}
