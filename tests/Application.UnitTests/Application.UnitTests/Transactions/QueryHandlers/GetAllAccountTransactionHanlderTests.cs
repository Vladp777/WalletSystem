using Application.Interfaces;
using Application.Repositories;
using Application.Transactions.CommandHandlers;
using Application.Transactions.Commands;
using Application.Transactions.Queries;
using Application.Transactions.QueryHandlers;
using Domain.Common.Errors;
using Domain.Entities;
using NSubstitute.ReturnsExtensions;

namespace Application.UnitTests.Transactions.QueryHandlers;

public class GetAllAccountTransactionHanlderTests
{
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();
    private readonly ITransactionRepository _transactionRepository = Substitute.For<ITransactionRepository>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();

    [Fact]
    public async Task Handle_ShouldReturnAccountNotFoundError_WhenInvalidAccountId()
    {
        var acccountId = Guid.NewGuid();

        _accountRepository.Get(acccountId).ReturnsNull();

        var command = new GetAllAccountTransaction(acccountId);
        var handler = new GetAllAccountTransactionHandler(_accountRepository, _transactionRepository, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.Account.AccountNotFound, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorizedError_WhenUserIdIsNotEquals()
    {
        var acccountId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var acccount = new Account
        {
            Id = acccountId,
            UserId = userId.ToString(),
        };

        _accountRepository.Get(acccountId).Returns(acccount);
        _currentUserService.UserId.Returns(Guid.NewGuid().ToString());

        var command = new GetAllAccountTransaction(acccountId);
        var handler = new GetAllAccountTransactionHandler(_accountRepository, _transactionRepository, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.User.Unauthorized, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnTransactionList_WhenDataIsCorrect()
    {
        var acccountId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var acccount = new Account
        {
            Id = acccountId,
            UserId = userId.ToString(),
        };

        _accountRepository.Get(acccountId).Returns(acccount);
        _currentUserService.UserId.Returns(userId.ToString());
        _transactionRepository.GetAll(acccountId).Returns(new List<Transaction> { new Transaction() });

        var command = new GetAllAccountTransaction(acccountId);
        var handler = new GetAllAccountTransactionHandler(_accountRepository, _transactionRepository, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.NotNull(result.Value);
        Assert.False(result.IsError);
    }
}
