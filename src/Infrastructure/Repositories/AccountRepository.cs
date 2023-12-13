using Application.Repositories;
using Domain.Entities;
using IdentityModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _context;
    
    public AccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public Task<Account> Create(Account entity)
    {
        _context.Accounts.Add(entity);

        return Task.FromResult(entity);
    }

    public Task<Account> Delete(Guid Id)
    {
        var deleted = _context.Accounts.First(x => x.Id == Id);

        _context.Accounts.Remove(deleted);

        return Task.FromResult(deleted);
    }

    public Task<Account?> Get(Guid id)
    {
        var result = _context.Accounts
            .Include(a => a.Transactions)
                .ThenInclude(t => t.Tag)
            .Include(a => a.Transactions)
                .ThenInclude(t => t.Type)
            .FirstOrDefault(x => x.Id == id);

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


    public async Task<Account> Update(Account entity)
    {
        var updatedEntity = _context.Accounts.First(x => x.Id == entity.Id);

        updatedEntity.Balance = entity.Balance;

        updatedEntity.Name = entity.Name;

        return updatedEntity;
    }
}
