using Application.Interfaces;
using Azure.Core;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Application.Models;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Infrastructure.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Infrastructure.Identity;

public class IdentityService: IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwtSettings;

    public IdentityService(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings;
    }

    public async Task<AuthenticationResult> RegisterUser(string email, string userName, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser != null)
        {
            return new AuthenticationResult(
                IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "Conflict",
                        Description = "Email is already in use"
                    })
                );
        }

        var newUser = new ApplicationUser
        {
            Email = email,
            Name = userName
        };

        var createdUser = await _userManager.CreateAsync(newUser, password);

        if (!createdUser.Succeeded)
        {
            return new AuthenticationResult(createdUser);
        }

        return GenerateAuthResultWithToken(newUser);
    }

    public async Task<AuthenticationResult> LoginUser(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return new AuthenticationResult(
                IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "Unauthorized",
                        Description = "Invalid email or password"
                    })
                );
        }

        var isUserHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!isUserHasValidPassword)
        {
            return new AuthenticationResult(
                    IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "Unauthorized",
                        Description = "Invalid email or password"
                    })
                );
        }

        return GenerateAuthResultWithToken(user);
    }

    private AuthenticationResult GenerateAuthResultWithToken(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sid, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            }),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthenticationResult
        {
            UserId = user.Id,
            Token = tokenHandler.WriteToken(token)

        };
    }
}
