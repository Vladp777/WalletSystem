using Application.Interfaces;
using Application.Repositories;
using Application.Transactions.CommandHandlers;
using Application.Transactions.Commands;
using Domain.Entities;
using Domain.Common.Errors;
using NSubstitute.ReturnsExtensions;

namespace Application.UnitTests.Transactions.CommandHandlers;

public class UpdateTransactionHandlerTests
{
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ITransactionRepository _transactionRepository = Substitute.For<ITransactionRepository>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();

    [Fact]
    public async Task Handle_ShouldReturnTransactionNotFoundError_WhenInvalidTransactionId()
    {
        var id = Guid.NewGuid();
        int typeId = 1;
        var desc = "";
        var date = DateTime.Now;


        _transactionRepository.Get(id).ReturnsNull();

        var command = new UpdateTransaction(id, desc, 0, date, 0);

        var handler = new UpdateTransactionHandler(_accountRepository, _transactionRepository, _unitOfWork, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.Transaction.TransactionNotFound, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnAccountNotFoundError_WhenInvalidAccountId()
    {
        var id = Guid.NewGuid();
        int typeId = 1;
        var desc = "";
        var date = DateTime.Now;
        var acccountId = Guid.NewGuid();

        var transaction = new Transaction
        {
            Id = id,
            AccountId = acccountId
        };

        _transactionRepository.Get(id).Returns(transaction);
        _accountRepository.Get(Guid.NewGuid()).ReturnsNull();

        var command = new UpdateTransaction(id, desc, 0, date, 0);

        var handler = new UpdateTransactionHandler(_accountRepository, _transactionRepository, _unitOfWork, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.Account.AccountNotFound, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorizedError_WhenUserIdNotEquals()
    {
        var id = Guid.NewGuid();
        var acccountId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        int typeId = 1;
        var desc = "";
        var date = DateTime.Now;

        var account = new Account
        {
            Id = id,
            UserId = userId.ToString()
        };

        var transaction = new Transaction
        {
            Id = id,
            AccountId = acccountId,
            TypeId = typeId,
            Description = desc,
            Count = 0,
            DateTime = date,
            Result_Balance = 0,
            TagId = 0
        };

        _transactionRepository.Get(id).Returns(transaction);
        _accountRepository.Get(acccountId).Returns(account);
        _currentUserService.UserId.Returns(Guid.NewGuid().ToString());

        var command = new UpdateTransaction(id, desc, 0, date, 0);


        var handler = new UpdateTransactionHandler(_accountRepository, _transactionRepository, _unitOfWork, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.User.Unauthorized, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnTransactionTagError_WhenWrongTag()
    {
        var id = Guid.NewGuid();
        var acccountId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        int typeId = 1;
        var desc = "";
        var date = DateTime.Now;

        var account = new Account
        {
            Id = id,
            UserId = userId.ToString()
        };

        var transaction = new Transaction
        {
            Id = id,
            AccountId = acccountId,
            TypeId = typeId,
            Description = desc,
            Count = 0,
            DateTime = date,
            Result_Balance = 0,
            TagId = 0
        };

        _transactionRepository.Get(id).Returns(transaction);
        _accountRepository.Get(acccountId).Returns(account);
        _currentUserService.UserId.Returns(userId.ToString());
        _transactionRepository.GetTransactionTags().Returns(new List<TransactionTag>());


        var command = new UpdateTransaction(id, desc, 0, date, 0);

        var handler = new UpdateTransactionHandler(_accountRepository, _transactionRepository, _unitOfWork, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.Transaction.WrongTransactionTag, result.Errors);
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
            UserId = userId,
            Balance = 1111
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

        _transactionRepository.Get(id).Returns(transaction);
        _accountRepository.Get(Arg.Any<Guid>()).Returns(account);
        _currentUserService.UserId.Returns(userId);
        _transactionRepository.GetTransactionTags().Returns(new List<TransactionTag>
        {
            new TransactionTag()
            {
                Id = default,
                Tag = "test"
            }
        });
        _transactionRepository.Update(Arg.Any<Transaction>()).Returns(transaction);

        var command = new UpdateTransaction(id, desc, 0, date, tagId);

        var handler = new UpdateTransactionHandler(_accountRepository, _transactionRepository, _unitOfWork, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.NotNull(result.Value);
        Assert.False(result.IsError);
        Assert.Equal(result.Value, transaction);
    }
}
