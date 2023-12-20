using Application.Authentication.Queries;
using Application.Interfaces;
using Application.Models;
using ErrorOr;
using MediatR;

namespace Application.Authentication.QueryHandlers;

public class LoginUserHandler : IRequestHandler<LoginUserQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IIdentityService _identityService;

    public LoginUserHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        return await _identityService.LoginUser(request.Email, request.Password);
    }
}
