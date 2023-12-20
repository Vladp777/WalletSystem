using Application.Accounts.CommandHandlers;
using Application.Accounts.Commands;
using Application.Repositories;
using Domain.Entities;

namespace Application.UnitTests.Accounts.CommandHandlers;

public class CreateAccountHandlerTests
{
    public readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();
    public readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    [Fact]
    public async Task Handle_ShouldReturnAccount_WhenDataIsCorrect()
    {
        var name = "Some name";
        double balance = 999;

        var account = new Account
        {
            Id = Guid.NewGuid(),
            Name = name,
            Balance = balance
        };

        _accountRepository.Create(Arg.Any<Account>()).Returns(account);

        var command = new CreateAccount(Guid.NewGuid(), name, balance);

        var handler = new CreateAccountHandler(_accountRepository, _unitOfWork);

        var result = await handler.Handle(command, default);


        Assert.NotNull(result.Value);
        Assert.Equal(result.Value.Name, name);
        Assert.Equal(result.Value.Balance, balance);
    }
}
