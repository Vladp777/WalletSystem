using Application.Authentication.Queries;
using Application.Authentication.QueryHandlers;
using Application.Interfaces;
using Application.Models;

namespace Application.UnitTests.Authentication.QueryHandlers;

public class LoginUserHandlerTests
{
    private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();

    [Fact]
    public async Task Handle_ShouldReturnErrorOrAuthResult()
    {
        var email = "email@gmail.com";
        //var name = "My name";
        var password = "password";

        var authResult = new AuthenticationResult
        {
            UserId = Guid.NewGuid().ToString(),
            Token = ""
        };

        _identityService.LoginUser(email, password).Returns(authResult);

        var command = new LoginUserQuery(email, password);

        var handler = new LoginUserHandler(_identityService);

        var result = await handler.Handle(command, default);

        Assert.NotNull(result.Value);
    }
}
