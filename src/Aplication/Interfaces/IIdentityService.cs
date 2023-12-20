using Application.Models;
using ErrorOr;

namespace Application.Interfaces;

public interface IIdentityService
{
    Task<ErrorOr<AuthenticationResult>> RegisterUser(string email, string userName, string password);
    Task<ErrorOr<AuthenticationResult>> LoginUser(string email, string password);
}
