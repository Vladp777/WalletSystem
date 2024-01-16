using Application.Repositories;
using DataGenerator;
using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Infrastructure.UnitTests.Repositories;

public class AccountRepositoryTests
{
    private readonly Mock<ApplicationDbContext> _context;
    private readonly Mock<DbSet<Account>> _dbSet;
    private readonly IAccountRepository _accountRepository;
    private readonly IEnumerable<Account> _accounts;
    

    public AccountRepositoryTests()
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

        _accountRepository = new AccountRepository(_context.Object);
    }

    [Fact]
    public async Task Create_ShouldAddEntity()
    {
        // Arrange
        var faker = new AutoGenerator<Account>();
        var account = faker.Generate();

        // Act
        var result = await _accountRepository.Create(account);

        // Assert
        Assert.Equal(account, result);
    }

    [Fact]
    public async Task Delete_ShouldRemoveEntity()
    {
        // Arrange
        var accountId = _accounts.First().Id;

        // Act
        var result = await _accountRepository.Delete(accountId);

        // Assert
        Assert.Equal(accountId, result.Id);
    }

    [Fact]
    public async Task Get_ShouldReturnAccount()
    {
        // Arrange

        var account = _accounts.First();

        // Act
        var result = await _accountRepository.Get(account.Id);

        // Assert
        Assert.Equal(account, result);
    }

    [Fact]
    public async Task Update_ShouldUpdateAccount()
    {
        // Arrange

        var account = _accounts.First();
        account.Name = "Test";

        // Act
        var result = await _accountRepository.Update(account);

        // Assert
        Assert.Equal(account, result);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAccounts()
    {
        // Arrange

        var account = _accounts.First();

        // Act
        var result = await _accountRepository.GetAll(Guid.Parse(account.UserId));

        // Assert
        Assert.Equal(account.UserId, result.First().UserId);
    }

}
