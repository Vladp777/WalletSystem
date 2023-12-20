using Application.Accounts.CommandHandlers;
using Application.Accounts.Commands;
using Application.Accounts.Queries;
using Application.Accounts.QueryHandlers;
using Application.Interfaces;
using Application.Repositories;
using Domain.Common.Errors;
using Domain.Entities;
using NSubstitute.ReturnsExtensions;

namespace Application.UnitTests.Accounts.QueryHandlers;

public class GetAccountByIdHandlerTests
{
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();

    [Fact]
    public async Task Handle_ShouldReturnAccountNotFoundError_WhenInvalidAccountId()
    {
        var id = Guid.NewGuid();

        _accountRepository.Get(id).ReturnsNull();

        var command = new GetAccountById(id);
        var handler = new GetAccountByIdHandler(_accountRepository, _currentUserService);

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

        var account = new Account
        {
            Id = id,
            UserId = userId
        };

        _accountRepository.Get(id).Returns(account);
        _currentUserService.UserId.Returns(Guid.NewGuid().ToString());

        var command = new GetAccountById(id);
        var handler = new GetAccountByIdHandler(_accountRepository, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.User.Unauthorized, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnAccount_WhenUserIdEquals()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();

        var account = new Account
        {
            Id = id,
            UserId = userId
        };

        _accountRepository.Get(id).Returns(account);
        _currentUserService.UserId.Returns(userId);

        var command = new GetAccountById(id);
        var handler = new GetAccountByIdHandler(_accountRepository, _currentUserService);

        var result = await handler.Handle(command, default);

        Assert.NotNull(result.Value);
        Assert.False(result.IsError);
        Assert.Equal(result.Value, account);
    }
}
