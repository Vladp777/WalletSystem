using Domain.Entities;


namespace Application.Repositories
{
    internal interface ITransactionRepository: IBaseRepository<Transaction>
    {
        Task<bool> DeleteAllTransactions(Guid accountId);
        Task<IEnumerable<Transaction>> GetTransactionsByTag(Guid accountId, TransactionTag tag);
        Task<IEnumerable<Transaction>> GetTransactionsByType(Guid accountId, TransactionType type);
        Task<IEnumerable<Transaction>> GetTransactionsByTypeAndTag(Guid accountId, TransactionType type, TransactionTag tag);
    }
}
