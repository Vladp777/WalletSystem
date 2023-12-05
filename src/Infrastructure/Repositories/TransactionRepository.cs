using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Transaction> Create(Transaction entity)
    {
        var result = _context.Transactions.Add(entity);

        await _unitOfWork.SaveAsync();

        return result.Entity;
    }

    public async Task<Transaction> Delete(Guid Id)
    {
        var deleted = _context.Transactions.First(x => x.Id == Id);

        _context.Transactions.Remove(deleted);

        await _unitOfWork.SaveAsync();

        return deleted;
    }
    public Task<Transaction?> Get(Guid id)
    {
        var transaction = _context.Transactions
            .Include(t => t.Tag)
            .Include(t => t.Type)
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

    //public Task<IEnumerable<Transaction>> GetTransactionsByTag(Guid accountId, TransactionTag tag)
    //{
    //    throw new NotImplementedException();
    //}

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

    public Task<List<Transaction>> GetTransactionsByTypeAndTag(Guid accountId, int typeId, int tagId)
    {
        var transactions = _context.Transactions
            .Include(t => t.Tag)
            .Include(t => t.Type)
            .Where(t => t.AccountId == accountId)
            .Where(t => t.TypeId == typeId 
                            && t.TagId == tagId)
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

    public async Task<Transaction> Update(Transaction entity)
    {
        var result = _context.Transactions.Update(entity);

        await _unitOfWork.SaveAsync();

        return result.Entity;
    }
}
