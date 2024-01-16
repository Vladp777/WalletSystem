using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Domain.Common.Errors;
using NSubstitute.ReturnsExtensions;
using Application.Transactions.Commands;
using Application.Transactions.CommandHandlers;

namespace Application.UnitTests.Transactions.CommandHandlers;

public class CreateTransactionHandlerTests
{
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ITransactionRepository _transactionRepository = Substitute.For<ITransactionRepository>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();

    [Fact]
    public async Task Handle_ShouldReturnAccountNotFoundError_WhenInvalidAccountId()
    {
        var id = Guid.NewGuid();
        int typeId = 1;
        var desc = "";
        var date = DateTime.Now;

        _accountRepository.Get(id).ReturnsNull();

        var command = new CreateTransaction(id,typeId, desc, 0, date, 0);
        var handler = new CreateTransactionHandler(_transactionRepository, _currentUserService, _accountRepository, _unitOfWork);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.Account.AccountNotFound, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorizedError_WhenUserIdNotEquals()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        int typeId = 1;
        var desc = "";
        var date = DateTime.Now;

        var account = new Account
        {
            Id = id,
            UserId = userId
        };

        _accountRepository.Get(id).Returns(account);
        _currentUserService.UserId.Returns(Guid.NewGuid().ToString());

        var command = new CreateTransaction(id, typeId, desc, 0, date, 0);
        var handler = new CreateTransactionHandler(_transactionRepository, _currentUserService, _accountRepository, _unitOfWork);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.User.Unauthorized, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnWrongTransactionTagError_WhenWrongTagId()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        int typeId = 1;
        var desc = "";
        var date = DateTime.Now;
        int tagId = default;

        var account = new Account
        {
            Id = id,
            UserId = userId
        };

        _accountRepository.Get(id).Returns(account);
        _currentUserService.UserId.Returns(userId);
        _transactionRepository.GetTransactionTags().Returns(new List<TransactionTag>());

        var command = new CreateTransaction(id, typeId, desc, 0, date, tagId);
        var handler = new CreateTransactionHandler(_transactionRepository, _currentUserService, _accountRepository, _unitOfWork);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.Transaction.WrongTransactionTag, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnWrongTransactionTypeError_WhenWrongTypeId()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        int typeId = 0;
        var desc = "";
        var date = DateTime.Now;
        int tagId = default;

        var account = new Account
        {
            Id = id,
            UserId = userId
        };

        _accountRepository.Get(id).Returns(account);
        _currentUserService.UserId.Returns(userId);
        //_transactionRepository.GetTransactionTags().Returns(new List<TransactionTag>());

        var command = new CreateTransaction(id, typeId, desc, 0, date, tagId);
        var handler = new CreateTransactionHandler(_transactionRepository, _currentUserService, _accountRepository, _unitOfWork);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.Transaction.WrongTransactionType, result.Errors);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Handle_ShouldReturnTransaction_WhenDataIsCorrect(int typeId)
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        var desc = "";
        var date = DateTime.Now;
        int tagId = default;

        var account = new Account
        {
            Id = id,
            UserId = userId
        };

        var transaction = new Transaction
        {
            Id = id,
            AccountId = Guid.NewGuid(),
            TypeId = typeId,
            Description = desc,
            Count = 0,
            DateTime = date,
            Result_Balance = 0,
            TagId = tagId
        };

        _accountRepository.Get(id).Returns(account);
        _currentUserService.UserId.Returns(userId);
        _transactionRepository.GetTransactionTags().Returns(new List<TransactionTag> 
        {
            new TransactionTag()
            {
                Id = default,
                Tag = "test"
            }
        });

        _transactionRepository.Create(Arg.Any<Transaction>()).Returns(transaction);


        var command = new CreateTransaction(id, typeId, desc, 0, date, tagId);
        var handler = new CreateTransactionHandler(_transactionRepository, _currentUserService, _accountRepository, _unitOfWork);

        var result = await handler.Handle(command, default);

        Assert.NotNull(result.Value);
        Assert.False(result.IsError);
        Assert.Equal(result.Value, transaction);
    }
}
