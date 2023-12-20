using DataGenerator;
using Domain.Entities;
using Domain.Common.Errors;
using Infrastructure.Identity;
using Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.UnitTests.Identity;

public class IdentityServiceTests
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwtSettings = Substitute.For<JwtSettings>();

    private readonly IdentityService _identityService;
    public IdentityServiceTests()
    {
        var userStore = Substitute.For<IUserStore<ApplicationUser>>();
        _userManager = Substitute.For<UserManager<ApplicationUser>>(userStore, null, null, null, null, null, null, null, null);
        _userManager.UserValidators.Add(new UserValidator<ApplicationUser>());
        _userManager.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

        _identityService = new IdentityService(_userManager, _jwtSettings);
    }


    [Fact]
    public async Task RegisterUser_ShouldReturnDuplicateEmailError_WhenEmailInUse()
    {
        var faker = new AutoGenerator<ApplicationUser>();

        var user = faker.Generate();

        _userManager.FindByEmailAsync(user.Email).Returns(user);

        var result = await _identityService.RegisterUser(user.Email, user.Name, user.PasswordHash);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.User.DuplicateEmail, result.Errors);
    }

    [Fact]
    public async Task RegisterUser_ShouldReturnAuthenticationResult_WhenRegistrationSucceeds()
    {
        var faker = new AutoGenerator<ApplicationUser>();
        var user = faker.Generate();

        _userManager.FindByEmailAsync(user.Email).ReturnsNull();
        _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(IdentityResult.Success);
        _jwtSettings.Secret = Guid.NewGuid().ToString();

        var result = await _identityService.RegisterUser(user.Email, user.Name, user.PasswordHash);

        Assert.NotNull(result.Value);
        Assert.False(result.IsError);
        Assert.NotNull(result.Value.Token);
    }

    [Fact]
    public async Task RegisterUser_ShouldReturnErrorList_WhenRegistrationIsFailed()
    {
        var faker = new AutoGenerator<ApplicationUser>();
        var user = faker.Generate();

        _userManager.FindByEmailAsync(user.Email).ReturnsNull();
        _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(IdentityResult.Failed(new IdentityError()));
        _jwtSettings.Secret = Guid.NewGuid().ToString();

        var result = await _identityService.RegisterUser(user.Email, user.Name, user.PasswordHash);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public async Task LoginUser_ShouldReturnAuthenticationResult_WhenLoginSucceeds()
    {
        var faker = new AutoGenerator<ApplicationUser>();
        var user = faker.Generate();

        _userManager.FindByEmailAsync(user.Email).Returns(user);
        _userManager.CheckPasswordAsync(user, Arg.Any<string>()).Returns(true);
        _jwtSettings.Secret = Guid.NewGuid().ToString();

        var result = await _identityService.LoginUser(user.Email, user.PasswordHash);

        Assert.NotNull(result.Value);
        Assert.False(result.IsError);
        Assert.Equal(user.Id, result.Value.UserId);
        Assert.NotNull(result.Value.Token);
    }

    [Fact]
    public async Task LoginUser_ShouldReturnInvalidEmailOrPassword_WhenUserNotFound()
    {
        var faker = new AutoGenerator<ApplicationUser>();
        var user = faker.Generate();

        _userManager.FindByEmailAsync(user.Email).ReturnsNull();

        var result = await _identityService.LoginUser(user.Email, user.PasswordHash);

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.User.InvalidEmailOrPassword, result.Errors);
    }

    [Fact]
    public async Task LoginUser_ShouldReturnInvalidEmailOrPassword_WhenPasswordIsIncorrect()
    {
        var faker = new AutoGenerator<ApplicationUser>();
        var user = faker.Generate();

        _userManager.FindByEmailAsync(user.Email).Returns(user);
        _userManager.CheckPasswordAsync(user, Arg.Any<string>()).Returns(false);

        var result = await _identityService.LoginUser(user.Email, "IncorrectPassword");

        Assert.Null(result.Value);
        Assert.NotEmpty(result.Errors);
        Assert.Contains(Errors.User.InvalidEmailOrPassword, result.Errors);
    }
}
