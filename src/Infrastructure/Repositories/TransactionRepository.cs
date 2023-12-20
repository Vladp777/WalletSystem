using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Transaction> Create(Transaction entity)
    {
        _context.Transactions.Add(entity);

        return Task.FromResult(entity);
    }

    public Task<Transaction> Delete(Guid Id)
    {
        var deleted = _context.Transactions.First(x => x.Id == Id);

        _context.Transactions.Remove(deleted);

        return Task.FromResult(deleted);
    }
    public Task<Transaction?> Get(Guid id)
    {
        var transaction = _context.Transactions
            .FirstOrDefault(x => x.Id == id);

        return Task.FromResult(transaction);
    }

    public Task<List<Transaction>> GetAll(Guid id)
    {
        var transactions = _context.Transactions
            .Include(t => t.Tag)
            .Include(t => t.Type)
            .Where(t => t.AccountId == id)
            .ToList();

        return Task.FromResult(transactions);
    }

    public Task<List<Transaction>> GetTransactionsByTypeAndPeriodDate(Guid accountId, int typeId, DateOnly fromDate, DateOnly toDate)
    {
        var transactions = _context.Transactions
            .Include(t => t.Tag)
            .Include(t => t.Type)
            .Where(t => t.AccountId == accountId)
            .Where(t => t.TypeId == typeId)
            .Where(t => t.DateTime >= fromDate.ToDateTime(TimeOnly.MinValue)
                            && t.DateTime <= toDate.ToDateTime(TimeOnly.MaxValue))
            .ToList();

        return Task.FromResult(transactions);
    }

    public Task<List<TransactionTag>> GetTransactionTags()
    {
        var transactionTags = _context.TransactionTags.ToList();

        return Task.FromResult(transactionTags);
    }
    public Task<List<Transaction>> GetTransactionsByTypeTagAndPeriodDate(Guid accountId, int typeId, int tagId, DateOnly fromDate, DateOnly toDate)
    {
        var transactions = _context.Transactions
            .Include(t => t.Tag)
            .Include(t => t.Type)
            .Where(t => t.AccountId == accountId)
            .Where(t => t.TypeId == typeId
                            && t.TagId == tagId)
            .Where(t => t.DateTime >= fromDate.ToDateTime(TimeOnly.MinValue) 
                            && t.DateTime <= toDate.ToDateTime(TimeOnly.MaxValue))
            .ToList();

        return Task.FromResult(transactions);
    }

    public Task<Transaction> Update(Transaction entity)
    {
        var trans = _context.Transactions.First(x => x.Id == entity.Id);
        if (trans == null)
            return null;

        trans.Result_Balance = entity.Result_Balance;
        trans.Description = entity.Description;
        trans.TagId = entity.TagId;
        trans.DateTime = entity.DateTime;
        trans.Count = entity.Count;

        var result = _context.Transactions.Update(trans);

        return Task.FromResult(trans);
    }
}
