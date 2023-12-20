using Application.Repositories;
using DataGenerator;
using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace Infrastructure.UnitTests.Repositories;

public class TransactionRepositoryTests
{
    private readonly Mock<ApplicationDbContext> _context;
    private readonly Mock<DbSet<Transaction>> _dbSetTransaction;
    private readonly Mock<DbSet<TransactionTag>> _dbSetTransactionTag;

    private readonly ITransactionRepository _transactionRepository;
    private readonly IEnumerable<Transaction> _transactions;
    private readonly IEnumerable<TransactionTag> _transactionTags;



    public TransactionRepositoryTests()
    {
        var faker = new AutoGenerator<Transaction>();

        _transactions = faker.Generate(10);

        var data = _transactions.AsQueryable();
        _context = new Mock<ApplicationDbContext>();

        _dbSetTransaction = new Mock<DbSet<Transaction>>();
        _dbSetTransaction.As<IQueryable<Transaction>>().Setup(m => m.Provider).Returns(data.Provider);
        _dbSetTransaction.As<IQueryable<Transaction>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _dbSetTransaction.As<IQueryable<Transaction>>().Setup(m => m.Expression).Returns(data.Expression);
        _dbSetTransaction.As<IQueryable<Transaction>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        _context.Setup(x => x.Transactions).Returns(_dbSetTransaction.Object);


        var tagsfaker = new AutoGenerator<TransactionTag>();
        _transactionTags = tagsfaker.Generate(10);

        var tags = _transactionTags.AsQueryable();

        _dbSetTransactionTag = new Mock<DbSet<TransactionTag>>();
        _dbSetTransactionTag.As<IQueryable<TransactionTag>>().Setup(m => m.Provider).Returns(tags.Provider);
        _dbSetTransactionTag.As<IQueryable<TransactionTag>>().Setup(m => m.ElementType).Returns(tags.ElementType);
        _dbSetTransactionTag.As<IQueryable<TransactionTag>>().Setup(m => m.Expression).Returns(tags.Expression);
        _dbSetTransactionTag.As<IQueryable<TransactionTag>>().Setup(m => m.GetEnumerator()).Returns(tags.GetEnumerator());
        _context.Setup(x => x.TransactionTags).Returns(_dbSetTransactionTag.Object);

        _transactionRepository = new TransactionRepository(_context.Object);
    }

    [Fact]
    public async Task Create_ShouldAddEntity()
    {
        // Arrange
        var faker = new AutoGenerator<Transaction>();
        var transaction = faker.Generate();

        // Act
        var result = await _transactionRepository.Create(transaction);

        // Assert
        Assert.Equal(transaction, result);
    }

    [Fact]
    public async Task Delete_ShouldRemoveEntity()
    {
        // Arrange
        var accountId = _transactions.First().Id;

        // Act
        var result = await _transactionRepository.Delete(accountId);

        // Assert
        Assert.Equal(accountId, result.Id);
    }

    [Fact]
    public async Task Get_ShouldReturnTransaction()
    {
        // Arrange

        var transaction = _transactions.First();

        // Act
        var result = await _transactionRepository.Get(transaction.Id);

        // Assert
        Assert.Equal(transaction, result);
    }

    [Fact]
    public async Task Update_ShouldUpdateTransaction()
    {
        // Arrange

        var transaction = _transactions.First();
        transaction.Description = "Test";
        _dbSetTransaction.Setup(a => a.Update(It.IsAny<Transaction>())).Returns((EntityEntry<Transaction>)null);
        // Act
        var result = await _transactionRepository.Update(transaction);

        // Assert
        Assert.Equal(transaction, result);
    }

    [Fact]
    public async Task GetAll_ShouldReturnTransactions()
    {
        // Arrange

        var account = _transactions.First();

        // Act
        var result = await _transactionRepository.GetAll(account.AccountId);

        // Assert
        Assert.Equal(account.AccountId, result.First().AccountId);
    }

    [Fact]
    public async Task GetTransactionsByTypeAndPeriodDate_ShouldReturnTransactions()
    {
        // Arrange

        var transaction = _transactions.First();

        // Act
        var result = await _transactionRepository.GetTransactionsByTypeAndPeriodDate(transaction.AccountId, transaction.TypeId, default, DateOnly.MaxValue);

        // Assert
        Assert.Equal(transaction.TypeId, result.First().TypeId);
    }

    [Fact]
    public async Task GetTransactionsByTypeTagAndPeriodDate_ShouldReturnTransactions()
    {
        // Arrange

        var transaction = _transactions.First();

        // Act
        var result = await _transactionRepository.GetTransactionsByTypeTagAndPeriodDate(transaction.AccountId, transaction.TypeId,transaction.TagId, default, DateOnly.MaxValue);

        // Assert
        foreach (var item in result)
        {
            Assert.Equal(transaction.TypeId, item.TypeId);
            Assert.Equal(transaction.TagId, item.TagId);
        }
    }

    [Fact]
    public async Task GetTransactionTags_ShouldReturnTransactions()
    {
        // Arrange

        // Act
        var result = await _transactionRepository.GetTransactionTags();

        // Assert

        Assert.Equal(_transactionTags, result);
    }
}
