using Application.Interfaces;
using Application.Repositories;
using Application.Transactions.CommandHandlers;
using Application.Transactions.Commands;
using Domain.Entities;
using Domain.Common.Errors;
using NSubstitute.ReturnsExtensions;
using System;

namespace Application.UnitTests.Transactions.CommandHandlers;

public class TransferHandlerTests
{
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ITransactionRepository _transactionRepository = Substitute.For<ITransactionRepository>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();

    [Fact]
    public async Task Handle_ShouldReturnAccountNotFoundError_WhenInvalidAccountId()
    {
        var fromId = Guid.NewGuid();
        var toId = Guid.NewGuid();
        double count = 1111;

        _accountRepository.Get(fromId).ReturnsNull();
        _accountRepository.Get(toId).ReturnsNull();

        var command = new TransferCommand(fromId, toId, count);
        var handler = new TransferHandler(_transactionRepository, _currentUserService, _accountRepository, _unitOfWork);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.Account.AccountNotFound, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorizedError_WhenUserIdNotEquals()
    {
        var fromId = Guid.NewGuid();
        var toId = Guid.NewGuid();
        double count = 1111;
        var userId = Guid.NewGuid();

        var from = new Account
        {
            Id = fromId,
            UserId = userId.ToString()
        };

        var to = new Account
        {
            Id = fromId,
            UserId = userId.ToString()
        };

        _accountRepository.Get(fromId).Returns(from);
        _accountRepository.Get(toId).Returns(to);
        _currentUserService.UserId.Returns(Guid.NewGuid().ToString());

        var command = new TransferCommand(fromId, toId, count);

        var handler = new TransferHandler(_transactionRepository, _currentUserService, _accountRepository, _unitOfWork);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.User.Unauthorized, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnTransaction_WhenDataIsCorrect()
    {
        var fromId = Guid.NewGuid();
        var toId = Guid.NewGuid();
        double count = 1111;
        var userId = Guid.NewGuid();
        var typeId = 2;
        var desc = "";
        var date = DateTime.Now;
        int tagId = default;
        var from = new Account
        {
            Id = fromId,
            UserId = userId.ToString()
        };

        var to = new Account
        {
            Id = fromId,
            UserId = userId.ToString()
        };

        var transaction = new Transaction
        {
            Id = fromId,
            AccountId = Guid.NewGuid(),
            TypeId = typeId,
            Description = desc,
            Count = 0,
            DateTime = date,
            Result_Balance = 0,
            TagId = tagId
        };


        _accountRepository.Get(fromId).Returns(from);
        _accountRepository.Get(toId).Returns(to);
        _currentUserService.UserId.Returns(userId.ToString());
        _transactionRepository.Create(Arg.Any<Transaction>()).Returns(transaction);

        var command = new TransferCommand(fromId, toId, count);

        var handler = new TransferHandler(_transactionRepository, _currentUserService, _accountRepository, _unitOfWork);

        var result = await handler.Handle(command, default);

        Assert.NotNull(result.Value);
        Assert.False(result.IsError);
        Assert.Equal(result.Value, transaction);
    }
}
