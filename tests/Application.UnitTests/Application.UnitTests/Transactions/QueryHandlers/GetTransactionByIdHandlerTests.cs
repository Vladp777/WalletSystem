using Application.Interfaces;
using Application.Repositories;
using Application.Transactions.Queries;
using Application.Transactions.QueryHandlers;
using Domain.Common.Errors;
using Domain.Entities;
using NSubstitute.ReturnsExtensions;

namespace Application.UnitTests.Transactions.QueryHandlers;

public class GetTransactionByIdHandlerTests
{
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();
    private readonly ITransactionRepository _transactionRepository = Substitute.For<ITransactionRepository>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();

    [Fact]
    public async Task Handle_ShouldReturnTransactionNotFoundError_WhenInvalidTransactionId()
    {
        var id = Guid.NewGuid();


        _transactionRepository.Get(id).ReturnsNull();

        var command = new GetTransactionById(id);
        var handler = new GetTransactionByIdHandler(_accountRepository, _transactionRepository, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.Transaction.TransactionNotFound, result.Errors);
    }
    [Fact]
    public async Task Handle_ShouldReturnAccountNotFoundError_WhenInvalidAccountId()
    {
        var id = Guid.NewGuid();
        var acccountId = Guid.NewGuid();

        var transaction = new Transaction
        {
            Id = id,
            AccountId = acccountId
        };

        _transactionRepository.Get(id).Returns(transaction);
        _accountRepository.Get(acccountId).ReturnsNull();

        var command = new GetTransactionById(id);
        var handler = new GetTransactionByIdHandler(_accountRepository, _transactionRepository, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.Account.AccountNotFound, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorizedError_WhenUserIdIsNotEquals()
    {
        var id = Guid.NewGuid();
        var accountId = Guid.NewGuid();

        var transaction = new Transaction
        {
            Id = id,
            AccountId = accountId
        };

        var account = new Account
        {
            Id = accountId,
            UserId = Guid.NewGuid().ToString(),
        };

        _transactionRepository.Get(id).Returns(transaction);
        _accountRepository.Get(accountId).Returns(account);
        _currentUserService.UserId.Returns(Guid.NewGuid().ToString());

        var command = new GetTransactionById(id);
        var handler = new GetTransactionByIdHandler(_accountRepository, _transactionRepository, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.User.Unauthorized, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnTransaction_WhenDataIsCorrect()
    {
        var id = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();

        var transaction = new Transaction
        {
            Id = id,
            AccountId = accountId
        };

        var account = new Account
        {
            Id = accountId,
            UserId = userId
        };

        _transactionRepository.Get(id).Returns(transaction);
        _accountRepository.Get(accountId).Returns(account);
        _currentUserService.UserId.Returns(userId);

        var command = new GetTransactionById(id);
        var handler = new GetTransactionByIdHandler(_accountRepository, _transactionRepository, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.NotNull(result.Value);
        Assert.False(result.IsError);
        Assert.Equal(transaction, result.Value);
    }
}
