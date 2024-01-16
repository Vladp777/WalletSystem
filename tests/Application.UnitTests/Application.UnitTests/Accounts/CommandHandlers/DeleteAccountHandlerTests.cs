using Application.Interfaces;
using Application.Repositories;
using Application.Accounts.CommandHandlers;
using Domain.Common.Errors;
using Application.Accounts.Commands;
using NSubstitute.ReturnsExtensions;
using Domain.Entities;

namespace Application.UnitTests.Accounts.CommandHandlers;

public class DeleteAccountHandlerTests
{
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();

    [Fact]

    public async Task Handle_ShouldReturnAccountNotFoundError_WhenInvalidAccountId()
    {
        var id = Guid.NewGuid();

        _accountRepository.Get(id).ReturnsNull();

        var command = new DeleteAccount(id);
        var handler = new DeleteAccountHandler(_accountRepository, _currentUserService);

        var result =await handler.Handle(command, default);

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

        var command = new DeleteAccount(id);

        var hanler = new DeleteAccountHandler(_accountRepository, _currentUserService);

        var result = await hanler.Handle(command, default);

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
        _accountRepository.Delete(id).Returns(account);
        _currentUserService.UserId.Returns(userId);

        var command = new DeleteAccount(id);

        var hanler = new DeleteAccountHandler(_accountRepository, _currentUserService);

        var result = await hanler.Handle(command, default);

        Assert.NotNull(result.Value);
        Assert.False(result.IsError);
        Assert.Equal(result.Value, account);    
    }
}
