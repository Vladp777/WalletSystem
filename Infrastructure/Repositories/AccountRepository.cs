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

    public async Task<Account?> Delete(Guid Id)
    {
        var deleted = _context.Accounts.FirstOrDefault(x => x.Id == Id);

        if (deleted == null)
            return null;

        _context.Accounts.Remove(deleted);

        await _unitOfWork.SaveAsync();

        return deleted;
    }

    public Task<Account?> Get(Guid id)
    {
        var result = _context.Accounts.Include(a => a.Transactions).FirstOrDefault(x => x.Id == id);
        
        return Task.FromResult(result);
    }

    public Task<IEnumerable<Account>> GetAll(Guid userId)
    {
        IEnumerable<Account> results = _context.Accounts.Include(a => a.Transactions).Where(x => x.UserId == userId.ToString());

        return Task.FromResult(results);
    }

    public async Task<Account?> Update(Account entity)
    {
        var updatedEntity = _context.Accounts.FirstOrDefault(x => x.Id == entity.Id);

        if (updatedEntity == null)
            return null;

        updatedEntity.Balance = entity.Balance;
        updatedEntity.Name = entity.Name;

        await _unitOfWork.SaveAsync();

        return updatedEntity;
    }
}
