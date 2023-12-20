using Application.Accounts.Queries;
using Application.Accounts.QueryHandlers;
using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Domain.Common.Errors;
using NSubstitute.ReturnsExtensions;

namespace Application.UnitTests.Accounts.QueryHandlers;

public class GetAllAccountsHandlerTests
{
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();

    [Fact]
    public async Task Handle_ShouldReturnUnauthorizedError_WhenUserIdNotEquals()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();

        //var getAllAcc = new GetAllAccounts(userId);

        //_accountRepository.Get(id).Returns(account);
        _currentUserService.UserId.Returns(Guid.NewGuid().ToString());

        var getAllAcc = new GetAllAccounts(userId);
        
        var handler = new GetAllAccountHandler(_accountRepository, _currentUserService);

        var result = await handler.Handle(getAllAcc, default);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.User.Unauthorized, result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnAccounts_WhenUserIsEquals()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var accounts = new List<Account>
        {
            new Account { Id = id, Name = "dff" }
        };

        _accountRepository.GetAll(userId).Returns(accounts);
        _currentUserService.UserId.Returns(userId.ToString());

        var getAllAcc = new GetAllAccounts(userId);

        var handler = new GetAllAccountHandler(_accountRepository, _currentUserService);

        var result = await handler.Handle(getAllAcc, default);

        Assert.NotNull(result.Value);
        Assert.False(result.IsError);
    }
}
