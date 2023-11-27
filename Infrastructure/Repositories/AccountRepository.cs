using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public AccountRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }
    public async Task<Account> Create(Account entity)
    {
        _context.Accounts.Add(entity);

        await _unitOfWork.SaveAsync();

        return entity;
    }

    public async Task<Account> Delete(Guid Id)
    {
        var deleted = _context.Accounts.First(x => x.Id == Id);

        _context.Accounts.Remove(deleted);

        await _unitOfWork.SaveAsync();

        return deleted;
    }

    public Task<Account> Get(Guid id)
    {
        var result = _context.Accounts
            .Include(a => a.Transactions)
                .ThenInclude(t => t.Tag)
            .Include(a => a.Transactions)
                .ThenInclude(t => t.Type)
            .First(x => x.Id == id);

        return Task.FromResult(result);
    }

    public Task<List<Account>> GetAll(Guid userId)
    {
        List<Account> results = _context.Accounts
            .Include(a => a.Transactions)
            .Where(x => x.UserId == userId.ToString())
            .ToList();

        return Task.FromResult(results);
    }

    public Task<Account> NoTrackingGet(Guid id)
    {
        var result = _context.Accounts.AsNoTracking().First(x => x.Id == id);

        return Task.FromResult(result);
    }

    public async Task<Account> Update(Account entity)
    {
        var updatedEntity = _context.Accounts.First(x => x.Id == entity.Id);

        updatedEntity.Balance = entity.Balance;

        updatedEntity.Name = entity.Name;

        await _unitOfWork.SaveAsync();

        return updatedEntity;
    }
}
