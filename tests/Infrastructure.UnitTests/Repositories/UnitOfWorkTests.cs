using Application.Repositories;
using DataGenerator;
using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitTests.Repositories;

public class UnitOfWorkTests
{
    private readonly Mock<ApplicationDbContext> _context;
    private readonly Mock<DbSet<Account>> _dbSet;
    private readonly IEnumerable<Account> _accounts;
    private readonly UnitOfWork _unitOfWork;


    public UnitOfWorkTests()
    {
        var faker = new AutoAccGenerator();

        _accounts = faker.Generate(10);

        var data = _accounts.AsQueryable();
        _context = new Mock<ApplicationDbContext>();

        _dbSet = new Mock<DbSet<Account>>();
        _dbSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(data.Provider);
        _dbSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _dbSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(data.Expression);
        _dbSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        _context.Setup(x => x.Accounts).Returns(_dbSet.Object);
        
        _unitOfWork = new UnitOfWork(_context.Object);
    }

    [Fact]
    public async Task SaveAsync_ShouldSave()
    {
        // Arrange

        var accountRepository = new AccountRepository(_context.Object);

        var faker = new AutoGenerator<Account>();
        var account = faker.Generate();

        var result = await accountRepository.Create(account);

        // Act
         await _unitOfWork.SaveAsync();

        // Assert
        _context.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

}
