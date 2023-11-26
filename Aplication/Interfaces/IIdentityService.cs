using Application.Models;

namespace Application.Interfaces;

public interface IIdentityService
{
    public Task<AuthenticationResult> RegisterUser(string email, string userName, string password);
    public Task<AuthenticationResult> LoginUser(string email, string password);
}
