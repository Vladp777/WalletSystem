using Application.Authentication.CommandHandlers;
using Application.Authentication.Commands;
using Application.Interfaces;
using Application.Models;

namespace Application.UnitTests.Authentication.CommandHandlers;

public class CreateUserHandlerTests
{
    private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();

    [Fact]
    public async Task Handle_ShouldReturnErrorOrAuthResult()
    {
        var email = "email@gmail.com";
        var name = "My name";
        var password = "password";

        var authResult = new AuthenticationResult
        {
            UserId = Guid.NewGuid().ToString(),
            Token = ""
        };

        _identityService.RegisterUser(email, name, password).Returns(authResult);

        var command = new CreateUserCommand(email, name, password);

        var handler = new CreateUserHandler(_identityService);

        var result = await handler.Handle(command, default);

        Assert.NotNull(result.Value);
    }

}
