using Application.Interfaces;
using Application.Reports.Queries;
using Application.Reports.QueryHandlers;
using Application.Repositories;
using Domain.Common.Errors;
using Domain.Entities;
using DataGenerator;
using NSubstitute.ReturnsExtensions;

namespace Application.UnitTests.Reports.QueryHandlers;

public class GetReportByTypeAndTagHandlerTests
{
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();
    private readonly ITransactionRepository _transactionRepository = Substitute.For<ITransactionRepository>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();

    [Fact]
    public async Task Handle_ShouldReturnAccountNotFoundError_WhenInvalidAccountId()
    {
        var faker = new AutoGenerator<Account>();

        var account = faker.Generate();

        _accountRepository.Get(account.Id).ReturnsNull();

        var command = new GetReportByTypeAndTag(account.Id, default, default, default, default);
        var handler = new GetReportByTypeAndTagHandler(_accountRepository, _transactionRepository, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.Account.AccountNotFound, result.Errors);
    }


    [Fact]
    public async Task Handle_ShouldReturnUnauthorizedError_WhenUserIdIsNotEquals()
    {
        var accountFaker = new AutoGenerator<Account>();

        var account = accountFaker.Generate();

        var transactionFaker = new AutoGenerator<Transaction>();

        var transaction = transactionFaker.Generate();

        transaction.AccountId = account.Id;


        _transactionRepository.Get(transaction.Id).Returns(transaction);
        _accountRepository.Get(account.Id).Returns(account);
        _currentUserService.UserId.Returns(Guid.NewGuid().ToString());


        var command = new GetReportByTypeAndTag(account.Id, default, default, default, default);
        var handler = new GetReportByTypeAndTagHandler(_accountRepository, _transactionRepository, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.User.Unauthorized, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnTransactionNotFoundError_WhenWrongData()
    {
        var accountFaker = new AutoGenerator<Account>();

        var account = accountFaker.Generate();

        var transactionFaker = new AutoGenerator<Transaction>();

        var transaction = transactionFaker.Generate();

        transaction.AccountId = account.Id;


        _transactionRepository.Get(transaction.Id).Returns(transaction);
        _accountRepository.Get(account.Id).Returns(account);
        _currentUserService.UserId.Returns(account.UserId);
        _transactionRepository.GetTransactionsByTypeTagAndPeriodDate(Arg.Any<Guid>(),
            Arg.Any<int>(),
            Arg.Any<int>(),
            Arg.Any<DateOnly>(),
            Arg.Any<DateOnly>())
            .ReturnsNull();

        var command = new GetReportByTypeAndTag(account.Id, default, default, default, default);
        var handler = new GetReportByTypeAndTagHandler(_accountRepository, _transactionRepository, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.Transaction.TransactionNotFound, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnReport_WhenDataIsCorrect()
    {
        var accountFaker = new AutoGenerator<Account>();
        var account = accountFaker.Generate();

        var transactionFaker = new AutoGenerator<Transaction>();
        var transaction = transactionFaker.Generate();
        transaction.AccountId = account.Id;

        _transactionRepository.Get(transaction.Id).Returns(transaction);
        _accountRepository.Get(account.Id).Returns(account);
        _currentUserService.UserId.Returns(account.UserId);
        _transactionRepository.GetTransactionsByTypeTagAndPeriodDate(Arg.Any<Guid>(),
            Arg.Any<int>(),
            Arg.Any<int>(),
            Arg.Any<DateOnly>(),
            Arg.Any<DateOnly>())
            .Returns(transactionFaker.Generate(5).ToList());

        var command = new GetReportByTypeAndTag(account.Id, default, default, default, default);
        var handler = new GetReportByTypeAndTagHandler(_accountRepository, _transactionRepository, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.NotNull(result.Value);
        Assert.False(result.IsError);
    }
}
