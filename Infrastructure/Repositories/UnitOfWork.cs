using Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Infrastructure;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public Task SaveAsync()
    {
        return _context.SaveChangesAsync();
    }
}
