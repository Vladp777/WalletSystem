using Domain.Entities;


namespace Application.Repositories
{
    public interface ITransactionRepository: IBaseRepository<Transaction>
    {
        //Task<bool> DeleteAllTransactions(Guid accountId);
        //Task<IEnumerable<Transaction>> GetTransactionsByTag(Guid accountId, TransactionTag tag);
        public Task<List<TransactionTag>> GetTransactionTags();
        Task<List<Transaction>> GetTransactionsByTypeAndPeriodDate(Guid accountId, int typeId, DateOnly fromDate, DateOnly toDate);
        Task<List<Transaction>> GetTransactionsByTypeAndTag(Guid accountId, int typeId, int tagId);
        Task<List<Transaction>> GetTransactionsByTypeTagAndPeriodDate(Guid accountId, int typeId, int tagId, DateOnly fromDate, DateOnly toDate);
        



    }
}
